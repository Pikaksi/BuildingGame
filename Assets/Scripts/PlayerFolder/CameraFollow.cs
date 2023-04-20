using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // [SerializeField] näyttää arvon unity editorissa scriptissä
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] Vector2 cameraRotation;
    [SerializeField] float sensitivity;

    public Transform player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //transform.position = player.position;
        //transform.position += cameraOffset;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        cameraRotation.x += mouseX * sensitivity;
        cameraRotation.y -= mouseY * sensitivity;
        transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f);
    }

    public Vector2 GetCameraRotation()
    {
        return cameraRotation;
    }
}