using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : PoolObject
{
    public static Action<Food> OnEaten;

    public void Eat()
    {
        OnEaten?.Invoke(this);

        gameObject.SetActive(false);
    }
}
