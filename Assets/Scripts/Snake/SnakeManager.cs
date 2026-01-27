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

    SnakePersonalization personalization;
    SnakeMovement movement;
    SnakeScore score;
    Animator animator;

    bool isMoving;
    bool canChangeDirection;
    ESnakeDirection currentDirection;

    public Action OnDied;
    public Action OnInitialGrow;

    public int InitialBodyCount => INITIAL_BODY_COUNT;

    private void Awake()
    {
        personalization = GetComponent<SnakePersonalization>();
        movement = GetComponent<SnakeMovement>();
        score = GetComponent<SnakeScore>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SetUp();

        score.OnBodyReached += Die;
        score.OnLimitReached += Die;
    }

    private void OnDisable()
    {
        score.OnBodyReached -= Die;
        score.OnLimitReached -= Die;
    }

    private void SetUp()
    {
        isMoving = false;
        canChangeDirection = true;
        currentDirection = ESnakeDirection.Up;
        transform.rotation = Quaternion.identity;
        movement.BodyParts.Clear();
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
    }

    IEnumerator MoveForwardCoroutine()
    {
        int growCounter = 0;

        while (isMoving == true)
        {
            float period = 1 / movesPerSecond;

            yield return new WaitForSeconds(period);
            movement.MoveForward();

            //  Direction can be changed only after the snake moved forward
            canChangeDirection = true;

            //  Make the snake grow after the game starts
            if(growCounter < INITIAL_BODY_COUNT)
            {
                OnInitialGrow?.Invoke();
                
                growCounter++;
            }
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
            personalization.SetHeadRotation(currentDirection);

            movement.ChangeDirection(doLocalTurnLeft.Value);

            //  After changing the direction, you can't change it again until the snake moves forward
            canChangeDirection = false;
        }

    }

    public void Grow(Body body)
    {
        movement.AddBodyPart(body);
    }

    public void SetInitialValues(float size, Vector2 position)
    {
        Vector3 scale = Vector3.one * size;

        transform.localScale = scale;
        transform.position = position;

        movement.SetInitialPosition();
    }

    public void SetSpeed(float speed)
    {
        movesPerSecond = speed;
    }

    public List<Transform> GetBody() 
    {
        List<Transform> result = new List<Transform>();

        for(int i = 0; i < movement.BodyParts.Count; i++)
        {
            result.Add(movement.BodyParts[i].transform);
        }

        result.Add(transform);

        return result;
    }

    public void Die()
    {
        OnDied?.Invoke();
        gameObject.SetActive(false);
    }
}

public enum ESnakeDirection { Invalid, Up, Down, Left, Right }