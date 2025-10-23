using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScore : MonoBehaviour
{
    public Action OnFoodReached;
    public Action OnBodyReached;
    public Action OnLimitReached;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food") == true)
        {
            var food = collision.GetComponent<Food>();
            if (food != null)
            {
                food.Eat();
            }
        }
        else if (collision.CompareTag("Body") == true)
        {
            OnBodyReached?.Invoke();
        }
        else if (collision.CompareTag("Limit") == true)
        {
            OnLimitReached?.Invoke();
        }
    }
}
