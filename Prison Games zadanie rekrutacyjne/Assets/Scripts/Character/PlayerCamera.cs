using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Control
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] float mouseSensitivity = 100f;
        [SerializeField] Transform playerBody = null;
        
        float xRotation = 0f;
        
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void LateUpdate()
        {
            CameraRotate();
        }
        
        void CameraRotate()
        {
            float sensitivity = mouseSensitivity * Time.deltaTime;

            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
