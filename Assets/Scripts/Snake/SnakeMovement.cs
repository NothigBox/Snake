using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] GameObject bodyPart;
    [SerializeField] List<Transform> bodyParts;

    Vector2 lastTailPosition;
    Quaternion lastTailRotation;

    public List<Transform> BodyParts => bodyParts;

    public void MoveForward()
    {
        Vector2 forwardPosition = transform.localPosition + transform.up * transform.localScale.x;

        //Debug.Log(forwardPosition);

        if (bodyParts.Count > 0)
        {
            lastTailPosition = bodyParts[bodyParts.Count - 1].position;
            lastTailRotation = bodyParts[bodyParts.Count - 1].rotation;

            for (int i = bodyParts.Count-1; i > 0; i--)
            {
                bodyParts[i].position = bodyParts[i-1].position;
                bodyParts[i].rotation = bodyParts[i - 1].rotation;
            }

            bodyParts[0].position = transform.position;
            bodyParts[0].rotation = transform.rotation;
        }
        else
        {
            lastTailPosition = transform.position;
            lastTailRotation = transform.rotation;
        }

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
        GameObject newBodyPart = Instantiate(bodyPart, lastTailPosition, lastTailRotation);
        newBodyPart.transform.localScale = transform.localScale;
        bodyParts.Add(newBodyPart.transform);
    }
}
