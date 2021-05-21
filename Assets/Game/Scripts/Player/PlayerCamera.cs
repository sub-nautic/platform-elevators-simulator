using System;
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
        [SerializeField] float mouseSensitivity = 75f;
        [SerializeField] Transform playerBody = null;
        [SerializeField] Camera playerCamera;

        [Header("Raycast config")]
        [SerializeField] float rayLenght = 2.5f;
        [SerializeField] LayerMask layerMaskInteractable;
        [SerializeField] Image uiCrosshair = null;

        GameObject raycastedObject;

        float xRotation = 0f;
        //bool isCrosshairDefault = true;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CrosshairNormal();
        }

        void Update()
        {
            //if (InteractWithUI()) return;
            if (InteractWitchComponent()) return;


            CrosshairNormal();
            CursorState();
            //InteractionRaycast();
        }

        bool InteractWitchComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        //SetCursor(raycastable.GetCursorType());
                        CrosshairActive();
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);

            RaycastHit[] hits = Physics.RaycastAll(playerCamera.transform.position, forward, rayLenght);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        void CrosshairActive()
        {
            uiCrosshair.color = Color.green;
        }

        void CrosshairNormal()
        {
            uiCrosshair.color = Color.white;
        }

        private static void CursorState()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.visible)
                {
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
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

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
