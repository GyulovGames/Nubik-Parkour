using UnityEngine;
using YG;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private float folowSpeed;
    [SerializeField] private Vector3 cameraOffset;
    [Space(5)]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform noobTransform;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = noobTransform.position + cameraOffset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, folowSpeed);
        transform.position = smoothPosition;
    }
}