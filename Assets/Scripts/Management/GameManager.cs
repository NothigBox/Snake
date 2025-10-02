using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TouchManager touch;
    [SerializeField] SnakeManager snake;

    private void Awake()
    {
        touch.OnChangeDirection += snake.TryToChangeDirection;
    }
}
