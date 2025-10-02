using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] GameObject bodyPart;
    [SerializeField] List<Transform> bodyParts;

    Vector2 lastTailPosition;

    private void Awake()
    {
        //bodyParts = new List<Transform>();
    }

    public void MoveForward()
    {
        Vector2 forwardPosition = transform.localPosition + transform.up * transform.localScale.x;

        //Debug.Log(forwardPosition);

        lastTailPosition = bodyParts[bodyParts.Count - 1].position;

        for (int i = bodyParts.Count-1; i > 0; i--)
        {
            bodyParts[i].position = bodyParts[i-1].position;
        }

        bodyParts[0].position = transform.position;

        transform.position = forwardPosition;
    }

    //  Rotate the head of the snake in a 90 degree angle to the left or to the right
    public void ChangeDirection(bool doTurnLeft)
    {
        float rotationDirection = doTurnLeft ? 1 : -1;

        transform.Rotate(Vector3.forward * (90f * rotationDirection));
    }

    public void AddBodyPart()
    {
        GameObject newBodyPart = Instantiate(bodyPart, lastTailPosition, Quaternion.identity);
        bodyParts.Add(newBodyPart.transform);
    }
}
