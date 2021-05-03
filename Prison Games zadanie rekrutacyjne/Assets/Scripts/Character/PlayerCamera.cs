using System.Collections;
using System.Collections.Generic;
using Project.Elevators;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Control
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Camera config")]
        [SerializeField] float mouseSensitivity = 100f;
        [SerializeField] Transform playerBody = null;

        [Header("Raycast config")]
        [SerializeField] float rayLenght = 2.5f;
        [SerializeField] LayerMask layerMaskInteractable;
        [SerializeField] Image uiCrosshair = null;
        
        GameObject raycastedObject;

        float xRotation = 0f;
        bool isCrosshairDefault = true;
        
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            CrosshairNormal();
        }

        void Update()
        {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            if(Physics.Raycast(transform.position, forward, out hit, rayLenght, layerMaskInteractable))
            {
                if(hit.collider.CompareTag("Object"))
                {
                    raycastedObject = hit.collider.gameObject;
                    isCrosshairDefault = false;
                    CrosshairActive();

                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        ElevatorCaller elevatorButton = raycastedObject.GetComponent<ElevatorCaller>();
                        
                        elevatorButton.CallElevator();
                    }
                }
            }
            else
            {
                if(!isCrosshairDefault)
                {
                    isCrosshairDefault = true;
                    CrosshairNormal();
                }                
            }
        }

        void CrosshairActive()
        {
            uiCrosshair.color = Color.green;
        }

        void CrosshairNormal()
        {
            uiCrosshair.color = Color.white;
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
