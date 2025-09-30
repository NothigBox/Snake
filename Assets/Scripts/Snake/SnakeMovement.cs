using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public void MoveForward()
    {
        Vector2 forwardPosition = transform.localPosition + transform.up * transform.localScale.x;

        Debug.Log(forwardPosition);

        transform.localPosition = forwardPosition;
    }

    public void ChangeDirection()
    {
        transform.Rotate(Vector3.forward * 90f);
    }
}
