using System.Collections;
using System.Collections.Generic;
using Project.Control;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpForce = 300f;
    [SerializeField] float horizontalDirectionSpeed = 0f;
    [SerializeField] float verticalDirectionSpeed = 0f;

    PlayerMover player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerMover playerMover = other.gameObject.GetComponent<PlayerMover>();
            Rigidbody playerRB = playerMover.GetComponent<Rigidbody>();

            StartCoroutine(playerMover.StopGroundCheck());
            playerMover.CanMove(false);
            playerRB.velocity = new Vector3(horizontalDirectionSpeed, 0, verticalDirectionSpeed);
            playerRB.AddForce(Vector3.up * jumpForce);
        }
    }
}
