using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] float movesPerSecond;

    SnakeMovement movement;
    bool isMoving;
    Vector2 startPosition;
    Vector2 endPosition;

    private void Awake()
    {
        movement = GetComponent<SnakeMovement>();
    }

    public void StartMoving()
    {
        if (isMoving == true)
        {
            return;
        }

        isMoving = true;
        StartCoroutine(MoveForwardCoroutine());
    }

    IEnumerator MoveForwardCoroutine()
    {
        while(isMoving == true)
        {
            yield return new WaitForSeconds(1/movesPerSecond);
            movement.MoveForward();
        }
    }

    void GetSwipeDirection()
    {

    }
}

public enum ESnakeDirection { Invalid, Up, Down, Left, Right }