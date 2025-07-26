using UnityEngine;
using System.Collections;

public class FaceCameraForward : MonoBehaviour
{
    public Transform playerBody;     // The root or main rotation part of your player
    public Transform cameraTransform; // Usually your Cinemachine virtual camera follow target
    public float turnSpeed = 5f;
    public float maxTurnAngle = 60f;

    void Update()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 playerForward = playerBody.forward;
        playerForward.y = 0f;

        float angle = Vector3.SignedAngle(playerForward, cameraForward, Vector3.up);

        if (Mathf.Abs(angle) > 2f && Mathf.Abs(angle) < maxTurnAngle)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }
}
