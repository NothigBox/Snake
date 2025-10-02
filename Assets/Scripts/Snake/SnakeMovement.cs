using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] List<Transform> bodyParts;

    private void Awake()
    {
        //bodyParts = new List<Transform>();
    }

    public void MoveForward()
    {
        Vector2 forwardPosition = transform.localPosition + transform.up * transform.localScale.x;

        //Debug.Log(forwardPosition);

        for (int i = bodyParts.Count-1; i > 0; i--)
        {
            bodyParts[i].localPosition = bodyParts[i-1].localPosition;
        }

        bodyParts[0].localPosition = transform.localPosition;

        transform.localPosition = forwardPosition;
    }

    //  Rotate the head of the snake in a 90 degree angle to the left or to the right
    public void ChangeDirection(bool doTurnLeft)
    {
        float rotationDirection = doTurnLeft ? 1 : -1;

        transform.Rotate(Vector3.forward * (90f * rotationDirection));
    }
}
