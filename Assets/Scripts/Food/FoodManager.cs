using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FoodSpawner))]
public class FoodManager : MonoBehaviour
{
    FoodSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<FoodSpawner>();
    }

    public void SpawnFood(Vector2 position)
    {
        spawner.SpawnApple(position);
    }
}
