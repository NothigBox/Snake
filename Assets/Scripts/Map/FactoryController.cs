using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    [SerializeField] Apple apple;
    [SerializeField] MapLimit limit;

    ObjectsPool<MapLimit> limitsPool;

    private void Awake()
    {
        limitsPool = new ObjectsPool<MapLimit>(limit);
    }

    public void GetBody()
    {

    }

    public void GetApple()
    {

    }

    public void GetKillWall()
    {

    }

    public MapLimit GetLimit(Vector2 position)
    {
        return limitsPool.GetObject(position);
    }
}
