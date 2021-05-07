using System.Collections;
using System.Collections.Generic;
using Project.Control;
using UnityEngine;

namespace Project.Pads
{
    public class JumpPad : MonoBehaviour
    {
        [SerializeField] float jumpForce = 300f;
        [SerializeField] float horizontalDirectionSpeed = 0f;
        [SerializeField] float verticalDirectionSpeed = 0f;

        PlayerMover playerMover;
        Rigidbody playerRB;

        bool isFired = false;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && !isFired)
            {
                playerMover = other.gameObject.GetComponent<PlayerMover>();
                playerRB = playerMover.GetComponent<Rigidbody>();

                StartCoroutine(playerMover.StopGroundCheck());
                playerMover.CanMove(false);

                playerMover.transform.position = transform.position + Vector3.up;

                playerRB.velocity = new Vector3(horizontalDirectionSpeed, 0, verticalDirectionSpeed);
                playerRB.AddForce(Vector3.up * jumpForce);
                isFired = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            playerMover = null;
            isFired = false;
        }
    }
}
