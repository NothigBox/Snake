using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TouchManager touch;
    [SerializeField] SnakeManager snake;

    private void OnEnable()
    {
        touch.OnChangeDirection += snake.TryToChangeDirection;
    }

    private void OnDisable()
    {
        touch.OnChangeDirection -= snake.TryToChangeDirection;
    }
}
