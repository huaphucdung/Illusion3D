using DG.Tweening;
using System;
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

    public Sequence MovePath(Player player, Walkable form, Walkable to)
    {
        Sequence sequence = DOTween.Sequence();
        float time = to.IsStar ? 1.5f : 1;
        sequence.Append(player.transform.DOMove(to.GetWalkPoint(), time * .2f).SetEase(Ease.Linear));
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
        sequence.Append(player.transform.DOMove(toWalkPoint, (IsUp ? 1 : time) * .2f).SetEase(Ease.Linear));
        return sequence;
    }
}