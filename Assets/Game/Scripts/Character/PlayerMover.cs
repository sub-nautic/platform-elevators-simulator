using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Control
{
    public class PlayerMover : MonoBehaviour
    {
        [Header("Movement Config")]
        [SerializeField] float forwardSpeedMultiplier = 1f;
        [SerializeField] float backwardSpeedMultiplier = 0.5f;
        [SerializeField] float sideSpeedMultiplier = 0.5f;
        [SerializeField] float walkSpeed = 5f;
        [SerializeField] float runSpeed = 8f;
        [SerializeField] float jumpHeight = 8.0f;

        [Header("Physics Config")]
        [Tooltip("How far throw raycast to check if player is grounded")]
        [SerializeField] float distToGround = 1f;

        [Header("Edge Lock Config")]
        [Tooltip("Choose layer who will check Emitter and GroundCheck")]
        [SerializeField] LayerMask groundLayer;
        [SerializeField] Transform groundCheckEmitter;
        [SerializeField] GroundCheck forwardGroundCheck;
        [SerializeField] GroundCheck backGroundCheck;
        [SerializeField] GroundCheck leftGroundCheck;
        [SerializeField] GroundCheck rightGroundCheck;

        [Header("UI Config")]
        [Tooltip("Add canvas with stats disply")]
        [SerializeField] StatsDisplay uiDisplay;

        GroundCheck currentDetector;
        Rigidbody playerRB;
        int emitterNum;
        bool isGrounded;

        bool groundCheckEnable = true;

        bool canMove = true;
        public bool CanMove(bool set) { return canMove = set; }

        void Awake()
        {
            playerRB = GetComponent<Rigidbody>();
        }

        void Start()
        {
            emitterNum = 1;
        }

        void Update()
        {
            HaveFootsOnGround();
            GroundCheck();
            Movement();
        }

        void Movement()
        {
            Jumping();
            Moving();
        }

        void Jumping()
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                StartCoroutine(StopGroundCheck());
                canMove = false;
                playerRB.velocity = new Vector3(playerRB.velocity.x, jumpHeight, playerRB.velocity.z);
                uiDisplay.AddJumpCounter();
            }
        }

        void Moving()
        {
            if (!canMove) return;

            float vertMove = Input.GetAxis("Vertical");
            float horizMove = Input.GetAxis("Horizontal");

            Vector3 moveVector = Vector3.zero;

            if (forwardGroundCheck.IsDetected() || backGroundCheck.IsDetected() ||
               rightGroundCheck.IsDetected() || leftGroundCheck.IsDetected() && !isGrounded)
            {
                if (vertMove > 0 && forwardGroundCheck.IsDetected()) { moveVector += transform.forward * forwardSpeedMultiplier; }
                if (vertMove < 0 && backGroundCheck.IsDetected()) { moveVector += -transform.forward * backwardSpeedMultiplier; }
                if (horizMove > 0 && rightGroundCheck.IsDetected()) { moveVector += transform.right * sideSpeedMultiplier; }
                if (horizMove < 0 && leftGroundCheck.IsDetected()) { moveVector += -transform.right * sideSpeedMultiplier; }
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveVector *= runSpeed;
            }
            else
            {
                moveVector *= walkSpeed;
            }

            playerRB.velocity = new Vector3(moveVector.x, playerRB.velocity.y, moveVector.z);
        }

        void GroundCheck()
        {
            UpdateEmitterNum();
            GetCurrentDetector();

            Vector3 origin = groundCheckEmitter.position;
            Vector3 direction = currentDetector.transform.position - groundCheckEmitter.position;
            float maxDistance = Vector3.Distance(currentDetector.transform.position, groundCheckEmitter.position);

            bool groundHit = Physics.Raycast(origin, direction, maxDistance, groundLayer);

            //If player jump, breaks GroundCheck
            if (!groundCheckEnable) groundHit = true;

            currentDetector.Detect(groundHit);
        }

        //Counts emitters by each frame
        void UpdateEmitterNum()
        {
            emitterNum += 1;
            if (emitterNum > 4)
            {
                emitterNum = 1;
            }
        }

        void GetCurrentDetector()
        {
            if (emitterNum == 1) { currentDetector = forwardGroundCheck; }
            if (emitterNum == 2) { currentDetector = backGroundCheck; }
            if (emitterNum == 3) { currentDetector = rightGroundCheck; }
            if (emitterNum == 4) { currentDetector = leftGroundCheck; }
        }

        //Raycast under player to check if can jump
        void HaveFootsOnGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + (Vector3.up * 0.05f), -Vector3.up, out hit, distToGround + 0.1f, groundLayer))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

        //Stopping checking edge till player hit ground
        public IEnumerator StopGroundCheck()
        {
            groundCheckEnable = false;

            yield return new WaitForSeconds(.2f);

            while (!groundCheckEnable)
            {
                if (isGrounded)
                {
                    groundCheckEnable = true;
                    canMove = true;
                }
                yield return null;
            }
        }
    }
}