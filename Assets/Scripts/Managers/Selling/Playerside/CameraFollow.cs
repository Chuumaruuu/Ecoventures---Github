using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float height = 3.35f;
    public float offset = 3f;

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(
                target.position.x, 
                target.position.y + height,
                 target.position.z - offset);
        }

        transform.rotation = Quaternion.Euler(20f, 0f, 0f);
    }
}
