using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.HableCurve;
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
        Debug.Log("Ladder Move");
        Vector3 fromWalkPoint = from.GetWalkPoint();
        Vector3 toWalkPoint = to.GetWalkPoint();
        Vector3 posBetween = IsUp ? fromWalkPoint : toWalkPoint;
       
        posBetween.y = IsUp ? toWalkPoint.y : fromWalkPoint.y;

        float time = Mathf.Abs(fromWalkPoint.y - toWalkPoint.y); 
        Sequence sequence = DOTween.Sequence();

        sequence.Append(player.transform.DOMove(posBetween, (IsUp ? time : 1) * .2f).SetEase(Ease.Linear));
        //TODO Set rotation

        sequence.Append(player.transform.DOMove(toWalkPoint, (IsUp ? 1 : time) * .2f).SetEase(Ease.Linear));
        //TODO Set rotation
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
        
        Walkable isNotStar = !from.IsStar ? from : to;
        posCenter += isNotStar.transform.up * valueOffset;

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
        Vector3 posCenter = (from.GetWalkPoint() + to.GetWalkPoint()) / 2;
        Vector3 moveDirection = (to.GetWalkPoint() - from.GetWalkPoint()).normalized;

        Vector3 fixedUp = to.transform.up;
        
        Vector3 right = Vector3.Cross(fixedUp, moveDirection).normalized;
        Vector3 forward = to.IsStar ? moveDirection : Vector3.Cross(right, fixedUp).normalized;

        Walkable isNotStar = !from.IsStar ? from : to;
        posCenter += isNotStar.transform.up * valueOffset;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(ReturnTime, SetMove, 1f, .2f).SetEase(Ease.Linear));
        void SetMove(float t)
        {
            player.transform.position = GetBezierPosition(t, from.GetWalkPoint(), posCenter, to.GetWalkPoint());
        }
        
        sequence.Join(player.transform.DORotateQuaternion(Quaternion.LookRotation(forward, fixedUp), 0.1f).SetEase(Ease.Linear));
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