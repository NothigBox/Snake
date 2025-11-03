using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    [SerializeField] private Body body;
    [SerializeField] private Apple apple;
    [SerializeField] private MapLimit limit;

    ObjectsPool<Body> bodyPool;
    ObjectsPool<Apple> applesPool;
    ObjectsPool<MapLimit> limitsPool;

    private void Awake()
    {
        bodyPool = new ObjectsPool<Body>(body);
        applesPool = new ObjectsPool<Apple>(apple);
        limitsPool = new ObjectsPool<MapLimit>(limit);
    }

    public Body GetBody(Vector2 position)
    {
        var result = bodyPool.GetObject(position);

        return result;
    }

    public Apple GetApple(Vector2 position)
    {
        return applesPool.GetObject(position);
    }

    public MapLimit GetLimit(Vector2 position)
    {
        return limitsPool.GetObject(position);
    }

    public void DeactivateAllObjects()
    {
        for (int i = 0; i < bodyPool.AllObjects.Count; i++)
        {
            bodyPool.AllObjects[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < applesPool.AllObjects.Count; i++)
        {
            applesPool.AllObjects[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < limitsPool.AllObjects.Count; i++)
        {
            limitsPool.AllObjects[i].gameObject.SetActive(false);
        }
    }
}
