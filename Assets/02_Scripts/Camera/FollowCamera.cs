using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    const float FOLLOW_THRESHOLD = 0.01f;
    const float CAMERA_Z_Offset = -10f;

    [SerializeField] float followSpeed;
    [SerializeField] float maxFollowDistance;

    Vector3 currentPosition = Vector3.zero;
    Vector3 targetPosition = Vector3.zero;

    Transform target = null;
    Transform my = null;

    public void Start()
    {
        target = transform.parent;
        my = transform;

        transform.parent = null;
    }

    private void LateUpdate()
    {
        targetPosition = target.position;

        targetPosition.z = CAMERA_Z_Offset;
        currentPosition.z = CAMERA_Z_Offset;

        currentPosition = Vector3.Lerp(currentPosition, targetPosition, followSpeed * Time.deltaTime);

        float distance = Vector3.Distance(currentPosition, targetPosition);

        if (distance >= FOLLOW_THRESHOLD)
        {
            if (distance > maxFollowDistance)
            {
                currentPosition = targetPosition + (currentPosition - targetPosition).normalized * maxFollowDistance;
            }

            my.position = currentPosition;
        }
    }
}
