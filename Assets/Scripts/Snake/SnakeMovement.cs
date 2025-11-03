using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] List<Body> bodyParts;

    Vector2 lastTailPosition;
    Quaternion lastTailRotation;

    public List<Body> BodyParts => bodyParts;

    public void SetInitialPosition()
    {
        SetLastTailInfo(transform);
    }

    //  Move the head of the snake towards Vector3.forward, and make its body follow it
    public void MoveForward()
    {
        Vector2 forwardPosition = transform.localPosition + transform.up * transform.localScale.x;

        if (bodyParts.Count > 0)
        {
            SetLastTailInfo(bodyParts[bodyParts.Count - 1].transform);

            //  Begin from the last body part, and make each one take the next-one's position and rotation
            for (int i = bodyParts.Count - 1; i > 0; i--)
            {
                bodyParts[i].SetPositionAndRotation(bodyParts[i - 1].transform);
            }

            // Make the nearest part to the head take the head's previous position and rotation
            bodyParts[0].SetPositionAndRotation(transform);
        }

        transform.position = forwardPosition;
    }

    //  Rotate the head of the snake in a 90 degree angle to the left or to the right
    public void ChangeDirection(bool doTurnLeft)
    {
        float rotationDirection = doTurnLeft ? 1 : -1;

        transform.Rotate(Vector3.forward * (90f * rotationDirection));
    }

    public void AddBodyPart(Body body)
    {
        if(bodyParts.Count > 0)
        {
            body.SetPositionAndRotation(lastTailPosition, lastTailRotation);
        }
        else
        {
            //  Make the first body part spawn with the current rotation of the head
            body.SetPositionAndRotation(lastTailPosition, transform.rotation);
        }

        bodyParts.Add(body);
    }

    private void SetLastTailInfo(Transform transform)
    {
        lastTailPosition = transform.position;
        lastTailRotation = transform.rotation;
    }
}
