using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] Apple apple;

    ObjectsPool<Apple> applePool;

    private void Awake()
    {
        applePool = new ObjectsPool<Apple>(apple);
    }

    public Apple SpawnApple(Vector2 position)
    {
        Apple newApple = Instantiate(apple, position, Quaternion.identity);
        return newApple;
    }
}
