using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    const int INITIAL_BODY_COUNT = 2;
    const float MIN_TIME_BETWEEN_TONGUE_ANIMATIONS = 5f;
    const float MAX_TIME_BETWEEN_TONGUE_ANIMATIONS = 10f;

    [SerializeField] float movesPerSecond;

    SnakeMovement movement;
    SnakeScore score;
    Animator animator;

    bool isMoving;
    bool canChangeDirection;
    ESnakeDirection currentDirection;

    public Action OnDied;

    private void Awake()
    {
        isMoving = false;
        canChangeDirection = true;
        currentDirection = ESnakeDirection.Up;
        movement = GetComponent<SnakeMovement>();
        score = GetComponent<SnakeScore>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        score.OnFoodReached += movement.AddBodyPart;
        score.OnBodyReached += Die;
        score.OnLimitReached += Die;
    }

    private void OnDisable()
    {
        score.OnFoodReached -= movement.AddBodyPart;
        score.OnBodyReached -= Die;
        score.OnLimitReached -= Die;
    }

    public void StartMoving()
    {
        if (isMoving == true)
        {
            return;
        }
        isMoving = true;

        DoTongueAnimation();
        StartCoroutine(MoveForwardCoroutine());

        //  Make the snake grow after the game starts
        for (int i = 0; i < INITIAL_BODY_COUNT; i++)
        {
            Grow();
        }
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

    void DoTongueAnimation()
    {
        if(isMoving == false)
        {
            return;
        }

        animator.SetTrigger("Tongue");

        //  Repeat this same function after a random range of time
        if(isMoving == true)
        {
            float random = UnityEngine.Random.Range(MIN_TIME_BETWEEN_TONGUE_ANIMATIONS, MAX_TIME_BETWEEN_TONGUE_ANIMATIONS);

            Invoke(nameof(DoTongueAnimation), random);
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

    public void Grow()
    {
        movement.AddBodyPart();
    }

    public void SetInitialValues(float size, Vector2 position)
    {
        Vector3 scale = Vector3.one * size;

        transform.localScale = scale;
        transform.position = position;
    }

    public List<Transform> GetBody() 
    {
        List<Transform> result = new List<Transform>();

        result.Add(transform);
        result.AddRange(movement.BodyParts);

        return result;
    }

    public void Die()
    {
        OnDied?.Invoke();
        gameObject.SetActive(false);
    }
}

public enum ESnakeDirection { Invalid, Up, Down, Left, Right }