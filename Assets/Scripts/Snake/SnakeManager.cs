using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] float movesPerSecond;

    SnakeMovement movement;
    SnakeScore score;

    bool isMoving;
    bool canChangeDirection;
    ESnakeDirection currentDirection;

    private void Awake()
    {
        isMoving = false;
        canChangeDirection = true;
        currentDirection = ESnakeDirection.Up;
        movement = GetComponent<SnakeMovement>();
        score = GetComponent<SnakeScore>();
    }

    private void OnEnable()
    {
        score.OnFoodReached += movement.AddBodyPart;
    }

    private void OnDisable()
    {
        score.OnFoodReached -= movement.AddBodyPart;
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
        while (isMoving == true)
        {
            float period = 1 / movesPerSecond;

            yield return new WaitForSeconds(period);
            movement.MoveForward();

            //  Direction can be changed only after the snake moved forward
            canChangeDirection = true;
        }
    }

    public void TryToChangeDirection(ESnakeDirection newDirection)
    {
        if(canChangeDirection == false)
        {
            return;
        }

        if(isMoving == false)
        {
            StartMoving();
        }

        if (currentDirection == newDirection)
        {
            return;
        }

        bool? doLocalTurnLeft = null;
        
        //  Only change the direction if it's traspassing between the Horizontal and the Vertical axis
        switch (currentDirection)
        {
            case ESnakeDirection.Left:
                switch (newDirection)
                {
                    case ESnakeDirection.Up:
                        doLocalTurnLeft = false;
                        currentDirection = ESnakeDirection.Up;
                        break;

                    case ESnakeDirection.Down:
                        doLocalTurnLeft = true;
                        currentDirection = ESnakeDirection.Down;
                        break;
                }
                break;

            case ESnakeDirection.Right:
                switch (newDirection)
                {
                    case ESnakeDirection.Up:
                        doLocalTurnLeft = true;
                        currentDirection = ESnakeDirection.Up;
                        break;

                    case ESnakeDirection.Down:
                        doLocalTurnLeft = false;
                        currentDirection = ESnakeDirection.Down;
                        break;
                }
                break;

            case ESnakeDirection.Up:
                switch (newDirection)
                {
                    case ESnakeDirection.Left:
                        doLocalTurnLeft = true;
                        currentDirection = ESnakeDirection.Left;
                        break;

                    case ESnakeDirection.Right:
                        doLocalTurnLeft = false;
                        currentDirection = ESnakeDirection.Right;
                        break;
                }
                break;

            case ESnakeDirection.Down:
                switch (newDirection)
                {
                    case ESnakeDirection.Left:
                        doLocalTurnLeft = false;
                        currentDirection = ESnakeDirection.Left;
                        break;

                    case ESnakeDirection.Right:
                        doLocalTurnLeft = true;
                        currentDirection = ESnakeDirection.Right;
                        break;
                }
                break;
        }

        if(doLocalTurnLeft != null)
        {
            movement.ChangeDirection(doLocalTurnLeft.Value);

            //  After changing the direction, you can't change it until the snake moves forward
            canChangeDirection = false;
        }

    }
}

public enum ESnakeDirection { Invalid, Up, Down, Left, Right }