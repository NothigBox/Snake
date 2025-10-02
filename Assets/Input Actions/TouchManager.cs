using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    const float SWIPE_THRESHOLD = 5f;

    PlayerInput input;

    InputAction changeDirection;

    public Action<ESnakeDirection> OnChangeDirection;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        changeDirection = input.actions["ChangeDirection"];
    }

    private void Start()
    {
        changeDirection.performed += ChangeDirection_performed;
    }

    private void OnDisable()
    {
        changeDirection.performed -= ChangeDirection_performed;
    }

    private void ChangeDirection_performed(InputAction.CallbackContext obj)
    {
        ESnakeDirection newDirection = ESnakeDirection.Invalid;
        Vector2 delta = obj.ReadValue<Vector2>();

        float absX = Mathf.Abs(delta.x);
        float absY = Mathf.Abs(delta.y);

        if(absX >= SWIPE_THRESHOLD || absY >= SWIPE_THRESHOLD)
        {
            //  Determine what axis is greater in magnitude
            if(absX != absY)
            {
                if (absX > absY)
                {
                    //  Movement is horizontal
                    if (delta.x < 0)
                    {
                        newDirection = ESnakeDirection.Left;
                    }
                    else if (delta.x > 0)
                    {
                        newDirection = ESnakeDirection.Right;
                    }
                }
                else 
                {
                    //  Movement is vertical
                    if (delta.y < 0)
                    {
                        newDirection = ESnakeDirection.Down;
                    }
                    else if (delta.y > 0)
                    {
                        newDirection = ESnakeDirection.Up;
                    }
                }
            }

            //Debug.Log($"Delta: {delta}");
            Debug.Log($"New Direction: {newDirection}");

            OnChangeDirection?.Invoke(newDirection);
        }
    }
}
