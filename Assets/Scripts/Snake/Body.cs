using UnityEngine;

public class Body : PoolObject
{
    public Vector3 position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public Quaternion rotation
    {
        get
        {
            return transform.rotation;
        }
        set
        {
            transform.rotation = value;
        }
    }

    public Vector3 localScale
    {
        get
        {
            return transform.localScale;
        }
        set
        {
            transform.localScale = value;
        }
    }

    public void SetPositionAndRotation(Transform transform)
    {
        SetPositionAndRotation(transform.position, transform.rotation);
    }

    public void SetPositionAndRotation(Vector3 position, Quaternion rotaion)
    {
        transform.position = position;
        transform.rotation = rotaion;
    }

    public void SetLocalScale(Vector3 localScale)
    {
        transform.localScale = localScale;
    }
}
