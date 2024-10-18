using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("Walk Points:")]
    [SerializeField] private Walkable startPoint;
    [SerializeField] private Walkable endPoint;

    public void Initiliaze(Player player)
    {
        player.SetPosition(startPoint);
    }

    public void Reset()
    {
       
    }
}
