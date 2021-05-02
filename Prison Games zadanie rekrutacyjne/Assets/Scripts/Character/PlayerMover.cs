using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Control
{
    [RequireComponent(typeof(CharacterController))]

    public class PlayerMover : MonoBehaviour
    {
        [Header("Movement Config")]
        [SerializeField] float speed = 5f;
        [SerializeField] float jumpHeight = 8.0f;
        
        [Header("Physics Config")]
        [SerializeField] float gravity = 20.0f;
        [SerializeField] float distToGround = 1f;

        [Header("Ledge Config")]
        [Tooltip("Choose layer who will check Emitter and GroundCheck")]
        [SerializeField] LayerMask groundLayer;
        [SerializeField] Transform groundCheckEmitter;
        [SerializeField] GroundCheck forwardGroundCheck;
        [SerializeField] GroundCheck rearGroundCheck;
        [SerializeField] GroundCheck leftGroundCheck;
        [SerializeField] GroundCheck rightGroundCheck;

        GroundCheck currentDetector;

        CharacterController characterController;
        Vector3 moveVector;
        
        int emitterNum;
        bool isGrounded;
        
        bool groundCheckEnable = true;

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        void Start()
        {
            //Reverse layerMask because the layerMask tells the raycast what to ignore. 
            //By reversing it, we make all the one 1's into 0's and all the 0's into 1's so it only pays attention to the ground.
            groundLayer = ~groundLayer;
            emitterNum = 1;
        }


        void Update()
        {            
            HaveFootsOnGround();
            UpdateEmitterNum();
            GroundCheck();
            Move();
        }


        void Move()
        {
            Vector3 moveVector = Vector3.zero;
            float vertMove = Input.GetAxis("Vertical");
            float horizMove = Input.GetAxis("Horizontal");

            if (isGrounded && moveVector.y < 0)
            {
                moveVector.y = 0f;
            }
            
            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                StartCoroutine(StopGroundCheck());
                moveVector.y = Mathf.Sqrt(jumpHeight * 3.0f * gravity);              
            }
            
            if (vertMove > 0 && forwardGroundCheck.IsDetected()) { moveVector += transform.forward; }
            if (vertMove < 0 && rearGroundCheck.IsDetected()) { moveVector += -transform.forward; }
            if (horizMove > 0 && rightGroundCheck.IsDetected()) { moveVector += transform.right; }
            if (horizMove < 0 && leftGroundCheck.IsDetected()) { moveVector += -transform.right; }

            moveVector.y -= gravity * Time.deltaTime;
              
            characterController.Move(moveVector * speed * Time.deltaTime);           
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
            if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.1f))
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
            yield return new WaitForSeconds(.1f);
            while (!isGrounded)
            {
                yield return null;
                if(isGrounded)
                {
                    groundCheckEnable = true;
                }
            }       
        }
    }
}