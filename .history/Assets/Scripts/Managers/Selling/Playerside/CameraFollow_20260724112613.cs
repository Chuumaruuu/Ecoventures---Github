using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private float height = 3.35f;
    private float offset = 3f;

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
