using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScore : MonoBehaviour
{
    public Action OnFoodReached;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Food") == true)
        {
            var a = collision.GetComponent<Food>();
            if(a != null)
            {
                a.Eat();
            }
        }
        else if(collision.CompareTag("Body") == true)
        {
            gameObject.SetActive(false);
        }
    }
}
