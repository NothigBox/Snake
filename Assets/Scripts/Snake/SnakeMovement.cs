using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public void MoveForward()
    {
        Vector2 forwardPosition = transform.localPosition + transform.up * transform.localScale.x;

        //Debug.Log(forwardPosition);

        transform.localPosition = forwardPosition;
    }

    //  Rotate the head of the snake in a 90 degree angle to the left or to the right
    public void ChangeDirection(bool doTurnLeft)
    {
        float rotationDirection = doTurnLeft ? 1 : -1;

        transform.Rotate(Vector3.forward * (90f * rotationDirection));
    }
}
