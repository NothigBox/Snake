using UnityEngine;

public class SnakePersonalization : MonoBehaviour
{
    [SerializeField] private Transform headPivot;

    public void SetHeadRotation(ESnakeDirection currentDirection)
    {
        return;

        Vector3 finalRotation = default;

        headPivot.rotation = Quaternion.identity;

        /*
        switch (currentDirection)
        {
            case ESnakeDirection.Up:
                finalRotation = new Vector3(0f, 0f, 0f);
                break;

            case ESnakeDirection.Down:
                finalRotation = new Vector3(0f, 0f, 180f);
                break;

            case ESnakeDirection.Left:
                finalRotation = new Vector3(0f, 0f, -90f);
                break;

            case ESnakeDirection.Right:
                finalRotation = new Vector3(0f, 0f, 90f);
                break;
        }

        Debug.Log(finalRotation);

        headPivot.eulerAngles = finalRotation;
        if (finalRotation != default)
        {
        }
        */
    }
}
