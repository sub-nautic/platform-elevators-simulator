using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Control
{
    public class PlayerMoverRB : MonoBehaviour
    {
        [Header("Movement Config")]
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
        [SerializeField] GroundCheck rearGroundCheck;
        [SerializeField] GroundCheck leftGroundCheck;
        [SerializeField] GroundCheck rightGroundCheck;

        [Header("UI Config")]
        [Tooltip("Add canvas with stats disply")]
        [SerializeField] StatsDisplay uiDisplay;

        Vector2 inputs;

        GroundCheck currentDetector;

        Rigidbody playerRB;

        int emitterNum;
        bool isGrounded;
        
        bool groundCheckEnable = true;

        void Awake()
        {
            playerRB = GetComponent<Rigidbody>();
        }

        void Start()
        {
            //Reverse layerMask because the layerMask tells the raycast what to ignore. 
            //By reversing it, we make all the one 1's into 0's and all the 0's into 1's so it only pays attention to the ground.
            //groundLayer = ~groundLayer;
            emitterNum = 1;
        }

        void Update()
        {            
            UpdateEmitterNum();
            GroundCheck();
            HaveFootsOnGround();
            Move();
        }
        
        void Move()
        {
            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                StartCoroutine(StopGroundCheck());
                playerRB.velocity = new Vector3(playerRB.velocity.x, jumpHeight, playerRB.velocity.z);
                uiDisplay.AddJumpCounter();            
            }

            

            float vertMove = Input.GetAxis("Vertical");
            float horizMove = Input.GetAxis("Horizontal");
            
            Vector3 moveVector = Vector3.zero;            
            if(!forwardGroundCheck.IsDetected() && !rearGroundCheck.IsDetected() &&
               !rightGroundCheck.IsDetected() && !leftGroundCheck.IsDetected()) return;
            else if(!isGrounded) return;

            if (vertMove > 0 && forwardGroundCheck.IsDetected()) { moveVector += transform.forward; }
            if (vertMove < 0 && rearGroundCheck.IsDetected()) { moveVector += -transform.forward; }
            if (horizMove > 0 && rightGroundCheck.IsDetected()) { moveVector += transform.right; }
            if (horizMove < 0 && leftGroundCheck.IsDetected()) { moveVector += -transform.right; }

            if(Input.GetKey(KeyCode.LeftShift))
            {
                moveVector *= runSpeed;
            }
            else
            {
                moveVector *= walkSpeed;
            }

            playerRB.velocity = new Vector3(moveVector.x, playerRB.velocity.y, moveVector.z);
        }

        void GetInputs()
        {
            inputs = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
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

        
        void GroundCheck()
        {            
            if (emitterNum == 1)
            {
                currentDetector = forwardGroundCheck;
            }
            if (emitterNum == 2)
            {
                currentDetector = rearGroundCheck;
            }
            if (emitterNum == 3)
            {
                currentDetector = rightGroundCheck;
            }
            if (emitterNum == 4)
            {
                currentDetector = leftGroundCheck;
            }

            Vector3 origin = groundCheckEmitter.position;
            Vector3 direction = currentDetector.transform.position - groundCheckEmitter.position;
            float maxDistance = Vector3.Distance(currentDetector.transform.position, groundCheckEmitter.position);

            bool groundHit = Physics.Raycast(origin, direction, maxDistance, groundLayer);

            //If player jump, breaks GroundCheck
            if(!groundCheckEnable)
            {
                groundHit = true;
            }
            
            currentDetector.Detect(groundHit);
        }

        //Raycast under player to check if can jump
        void HaveFootsOnGround()
        {
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.1f))
            if (Physics.Raycast(transform.position + (Vector3.up * 0.05f), -Vector3.up, out hit, distToGround + 0.1f))
            {
                isGrounded = true;
                print("grounded");
            }
            else
            {
                isGrounded = false;
                print("not grounded");
            }
        }

        //Stopping checking edge till player hit ground
        IEnumerator StopGroundCheck()
        {           
            groundCheckEnable = false;
            yield return new WaitForSeconds(.2f);
            while (!isGrounded)
            {
                groundCheckEnable = true;
                yield return null;
            }
        }     
    }
}