using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
public interface IPathCommand
{
    void DrawGizmod(Walkable from,  Walkable to);
    Sequence MovePath(Player player, Walkable from, Walkable to);
}

[Serializable]
public class WalkPathCommand : IPathCommand
{
    public void DrawGizmod(Walkable from, Walkable to)
    {
        Gizmos.DrawLine(from.GetWalkPoint(), to.GetWalkPoint());
    }

    public Sequence MovePath(Player player, Walkable from, Walkable to)
    {
        Sequence sequence = DOTween.Sequence();
        float time = to.IsStar ? 1.5f : 1;
        
        Vector3 fromWalkPoint = from.GetWalkPoint();
        Vector3 toWalkPoint = to.GetWalkPoint();
        
        Vector3 moveDirection = (toWalkPoint - fromWalkPoint).normalized;
        Vector3 fixedUp = to.transform.up;
        Vector3 right = Vector3.Cross(fixedUp, moveDirection).normalized;
        Vector3 forward = Vector3.Cross(right, fixedUp).normalized;

        float maxInVector = Mathf.Max(Mathf.Abs(forward.x), Mathf.Abs(forward.y), Mathf.Abs(forward.z));
        forward = new Vector3(
            Mathf.Abs(forward.x) == maxInVector ? forward.x : 0,
            Mathf.Abs(forward.y) == maxInVector ? forward.y : 0,
            Mathf.Abs(forward.z) == maxInVector ? forward.z : 0
            ).normalized;

        sequence.Append(player.transform.DOMove(to.GetWalkPoint(), time * .2f).SetEase(Ease.Linear));
        sequence.Join(player.transform.DORotateQuaternion(Quaternion.LookRotation(forward, fixedUp), 0.1f).SetEase(Ease.Linear));
        return sequence;
    }
}

[Serializable]
public class LadderPathCommand : IPathCommand
{
    public bool IsUp;
    public void DrawGizmod(Walkable from, Walkable to)
    {
        Vector3 fromWalkPoint = from.GetWalkPoint();
        Vector3 toWalkPoint = to.GetWalkPoint();
        Vector3 posBetween = IsUp ? fromWalkPoint : toWalkPoint;
        
        posBetween.y = IsUp ? toWalkPoint.y : fromWalkPoint.y; 

        Gizmos.DrawLine(fromWalkPoint, posBetween);
        Gizmos.DrawLine(posBetween, toWalkPoint);
    }

    public Sequence MovePath(Player player, Walkable from, Walkable to)
    {
        Vector3 fromWalkPoint = from.GetWalkPoint();
        Vector3 toWalkPoint = to.GetWalkPoint();
        Vector3 posBetween = IsUp ? fromWalkPoint : toWalkPoint;

        Vector3 moveDirection = (to.GetWalkPoint() - from.GetWalkPoint()).normalized;
        Vector3 fixedUp = to.transform.up;
        Vector3 right = Vector3.Cross(fixedUp, moveDirection).normalized;
        Vector3 forward = Vector3.Cross(right, fixedUp).normalized;

        posBetween.y = IsUp ? toWalkPoint.y : fromWalkPoint.y;

        float time = Mathf.Abs(fromWalkPoint.y - toWalkPoint.y); 
        Sequence sequence = DOTween.Sequence();

        sequence.Append(player.transform.DOMove(posBetween, (IsUp ? time : 1) * .2f).SetEase(Ease.Linear));
        sequence.Join(player.transform.DORotateQuaternion(Quaternion.LookRotation(forward, fixedUp), 0.1f).SetEase(Ease.Linear));

        sequence.Append(player.transform.DOMove(toWalkPoint, (IsUp ? 1 : time) * .2f).SetEase(Ease.Linear));
        sequence.Join(player.transform.DORotateQuaternion(Quaternion.LookRotation(IsUp ? forward : -forward, fixedUp), 0.1f).SetEase(Ease.Linear));
        return sequence;
    }
}

[Serializable]
public class BezierPathCommand : IPathCommand
{
    public float valueOffset; 
    // Number of segments to draw the curve
    private int segments = 20;
    public void DrawGizmod(Walkable from, Walkable to)
    {
        if (from == null || to == null) return;

        Gizmos.color = Color.green;

        Vector3 previousPoint = from.GetWalkPoint();
        Vector3 posCenter = (from.GetWalkPoint() + to.GetWalkPoint()) / 2;
        
        posCenter += valueOffset *(from.transform.up + to.transform.up);

        // Loop through the segments and draw the curve
        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 currentPoint = GetBezierPosition(t, from.GetWalkPoint(), posCenter, to.GetWalkPoint());
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }

        // Optionally, draw the control points as spheres
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(from.GetWalkPoint(), 0.1f);
        Gizmos.DrawSphere(to.GetWalkPoint(), 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(posCenter, 0.1f);
    }

    public Sequence MovePath(Player player, Walkable from, Walkable to)
    {
        Vector3 moveDirection = (to.GetWalkPoint() - from.GetWalkPoint()).normalized;
        Vector3 posCenter = (from.GetWalkPoint() + to.GetWalkPoint()) / 2;

        Vector3 fixedUp = to.transform.up;
        
        Vector3 right = Vector3.Cross(fixedUp, moveDirection).normalized;
        Vector3 forward = Vector3.Cross(right, fixedUp).normalized;

        posCenter += valueOffset * (from.transform.up + to.transform.up);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(ReturnTime, SetMove, 1f, .3f).SetEase(Ease.Linear));
        void SetMove(float t)
        {
            player.transform.position = GetBezierPosition(t, from.GetWalkPoint(), posCenter, to.GetWalkPoint());
        }
        
        sequence.Join(player.transform.DORotateQuaternion(Quaternion.LookRotation(forward, fixedUp), .3f).SetEase(Ease.Linear));
        return sequence;
    }

    private Vector3 GetBezierPosition(float t, Vector3 pos0, Vector3 pos1, Vector3 pos2)
    {
        // Quadratic Bezier curve formula
        return (1 - t) * (1 - t) * pos0 + 2 * (1 - t) * t * pos1 + t * t * pos2;
    }

    private float ReturnTime()
    {
        return 0f;
    }
}

[Serializable]
public class CirclePath : IPathCommand
{
    public CircleType type;
    public bool flip = false;

    private Dictionary<CircleType, float> angleDictionary = new Dictionary<CircleType, float>()
        {{CircleType.Half, 180f },
        {CircleType.Quater, 90f }};

    // Number of segments to draw the curve
    private int segments = 20;

    public void DrawGizmod(Walkable from, Walkable to)
    {
        if (from == null || to == null) return;

        Vector3 midPoint = CacculatePointCenter(from.GetWalkPoint(), to.GetWalkPoint(), angleDictionary[type], from.transform.up, flip);
        float radius = Vector3.Distance(from.GetWalkPoint(), midPoint);

        // Vector from A to B
        Vector3 direction = (to.GetWalkPoint() - from.GetWalkPoint()).normalized;

        Vector3 previousPoint = from.GetWalkPoint();
        float angleStep = angleDictionary[type] / segments;
        
        // Draw the arc path
        for (int i = 1; i <= segments; i++)
        {
            float angleForPoint = i * angleStep;
            Vector3 currentPos = CalculatePointC(midPoint, radius, from.GetWalkPoint(), angleForPoint, from.transform.up, flip);
            Gizmos.DrawLine(previousPoint, currentPos);
            previousPoint = currentPos;
        }

        // Optionally, draw the control points as spheres
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(from.GetWalkPoint(), 0.1f);
        Gizmos.DrawSphere(to.GetWalkPoint(), 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(midPoint, 0.1f);
    }

    public Sequence MovePath(Player player, Walkable from, Walkable to)
    {
        Vector3 midPoint = CacculatePointCenter(from.GetWalkPoint(), to.GetWalkPoint(), angleDictionary[type], from.transform.up, flip);
        float radius = Vector3.Distance(from.GetWalkPoint(), midPoint);

        // Vector from A to B
        Vector3 previousPoint = from.GetWalkPoint();
        Vector3 fixedUp = to.transform.up;


        float flipValue = flip ? 1 : -1;
        Vector3 directionToCenter = (midPoint - from.GetWalkPoint()).normalized;
        Vector3 startDirection = Vector3.Cross(directionToCenter, fixedUp * flipValue); 
        Quaternion rotation = Quaternion.Euler(fixedUp * flipValue * (angleDictionary[type]));
        Vector3 endDirectoin = rotation * startDirection;

        //TODO find rotaion direction left or right 


        Sequence sequence = DOTween.Sequence();
        float durationByAngle = angleDictionary[type] / 90;

        /*sequence.Join(player.transform.DORotateQuaternion(Quaternion.LookRotation(moveDirection * flipValue, fixedUp), .1f)
            .SetEase(Ease.Linear));*/

        sequence.Append(DOTween.To(ReturnTime, SetMove, 1f, durationByAngle * .3f).SetEase(Ease.Linear));
        void SetMove(float t)
        {
            float angleForPoint = t * angleDictionary[type];
            player.transform.position = CalculatePointC(midPoint, radius, from.GetWalkPoint(), angleForPoint, fixedUp, flip);
        }
        sequence.Join(player.transform.DORotateQuaternion(Quaternion.LookRotation(endDirectoin, fixedUp), durationByAngle * .3f)
            .SetEase(Ease.Linear));

        return sequence;
    }

    private Vector3 CalculatePointC(Vector3 center, float radius, Vector3 pointA, float angle, Vector3 direction, bool flip)
    {
        float flipValue = flip ? 1f : -1f;
        // Convert angleAOC from degrees to radians
        float angleAOC_Radians = angle * Mathf.Deg2Rad;

        // Calculate the direction from the center to point A
        Vector3 directionToA = (pointA - center).normalized;

        // Create a rotation quaternion based on the normal vector and angleAOC
        Quaternion rotation = Quaternion.AngleAxis(angle, direction * flipValue);

        // Rotate the direction vector by the quaternion to get the direction of point C
        Vector3 directionToC = rotation * directionToA;

        // Calculate the position of point C based on the rotated direction vector and radius
        Vector3 pointC = center + directionToC * radius ;

        return pointC;
    }

    private Vector3 CacculatePointCenter(Vector3 pointA, Vector3 pointB, float angle, Vector3 direction, bool flip)
    {
        if (angle == 180) return (pointA + pointB) / 2;

        int flipValue = flip ? -1 : 1;
        // Step 1: Calculate the midpoint M between points A and B
        Vector3 midpoint = (pointA + pointB) / 2;

        // Step 2: Calculate the direction vector from A to B
        Vector3 AB_Direction = pointB - pointA;

        // Step 3: Find a perpendicular vector to AB using the cross product
        Vector3 perpendicularVector = Vector3.Cross(AB_Direction, direction * flipValue).normalized;

        // Step 4: Calculate the radius of the circle using the given central angle formula
        float distanceAB = AB_Direction.magnitude;

        // Calculate the radius based on the formula:
        float radius = distanceAB / (2 * Mathf.Round(Mathf.Sin(angle / 2))); // Use the provided angle AOB in radians

        // Step 5: Calculate the center of the circle by moving from the midpoint in the perpendicular direction
        // You can move in either direction along the perpendicular vector to find two possible centers.
        Vector3 center = midpoint + perpendicularVector * radius;

        return center;
    }

    private float ReturnTime()
    {
        return 0f;
    }
}

public enum CircleType
{
    Half,
    Quater
}