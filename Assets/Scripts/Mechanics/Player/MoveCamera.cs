using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float Sensitive = 1.3f;
    private Quaternion CameraRotation;
    private Camera cam;

    [SerializeField]
    private float MaxX = 80.0f;
    [SerializeField]
    private float MinX = -80.0f;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (GameState.IsPaused)
            return;

        float rotateX = Input.GetAxis("Mouse Y") * Sensitive;
        float rotateY = Input.GetAxis("Mouse X") * Sensitive;

        transform.localRotation *= Quaternion.Euler(0f, rotateY, 0f);
        cam.transform.localRotation *= Quaternion.Euler(-rotateX, 0f, 0f);
        cam.transform.localRotation = ClampRotate(cam.transform.localRotation);
    }

    private Quaternion ClampRotate(Quaternion rot)
    {
        rot.x /= rot.w;
        rot.y /= rot.w;
        rot.z /= rot.w;
        rot.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rot.x);

        angleX = Mathf.Clamp(angleX, MinX, MaxX);

        rot.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return rot;
    }
}
