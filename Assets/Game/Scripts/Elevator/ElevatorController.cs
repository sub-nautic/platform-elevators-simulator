using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Elevators
{
    [RequireComponent(typeof(ElevatorMover), typeof(ElevatorPlayerCheck))]
    public class ElevatorController : MonoBehaviour
    {
        [Tooltip("Select true if this gameObject is a moving platform")]
        [SerializeField] bool isPlatform;
        [SerializeField] float elevatorSpeedFraction = 0.01f;
        [SerializeField] float dwellingAtFloorTime = 1f;
        [SerializeField] float waitInIdleState = 10f;
        [SerializeField] ElevatorPath elevatorPath;
        
        Vector3 elevatorPos;

        ElevatorMover elevatorMover;
        Vector3 calledElevatorDestination;
        
        float elevatorTolerance = 1f;

        float timeSinceArrivedAtFloor = Mathf.Infinity;
        int currentFloorIndex = 0;
        bool isElevatorCalled = false;

        int selectedDestination;
        
        void Awake()
        {
            elevatorMover = GetComponent<ElevatorMover>();

            elevatorPos = transform.position;
        }

        void Start()
        {
            if(isPlatform) elevatorTolerance = 0.0001f;
            else elevatorTolerance = 1f;
        }

        void Update()
        {
            if(isElevatorCalled)
            {
                MoveToPlayerSelectedFloor(selectedDestination);
            }
            else
            {
                ElevatorBehaviour();
            }
            //print(gameObject.name +" called: " + isElevatorCalled);

            timeSinceArrivedAtFloor += Time.deltaTime;
        }
        
        public bool IsElevatorCalled(bool isCalled){ return isElevatorCalled = isCalled; }
        public void MoveToPlayerSelectedFloor(int floorIndex)
        {            
            selectedDestination = floorIndex;
            if(!isPlatform)
            {
                calledElevatorDestination = GetCurrentFloor(selectedDestination) - Vector3.up;
                //print("Elevator is called");
            }
            else
            {
                calledElevatorDestination = GetCurrentFloor(selectedDestination);
                //print("Platform is called");
            }

            elevatorMover.StartMoveAction(calledElevatorDestination, elevatorSpeedFraction);

            Vector3 newPosition = calledElevatorDestination;

            //When elevator will be on destination floor time will start counting
            if (newPosition != transform.position){ timeSinceArrivedAtFloor = 0; }
            
            //If in destination position            
            if(newPosition == transform.position)
            {
                if(timeSinceArrivedAtFloor > waitInIdleState)
                {
                    isElevatorCalled = false;
                }                
                //print(gameObject.name + " - Im at destination point");
            }
        }

        void ElevatorBehaviour()
        {
            Vector3 nextPosition = elevatorPos;

            if (elevatorPath != null)
            {
                if (AtFloor())
                {
                    timeSinceArrivedAtFloor = 0;
                    CycleFloor();
                }
                nextPosition = GetCurrentFloor();                
            }
            if (timeSinceArrivedAtFloor > dwellingAtFloorTime)
            {
                elevatorMover.StartMoveAction(nextPosition, elevatorSpeedFraction);
            }
        }

        bool AtFloor()
        {
            float distanceToPoint = Vector3.Distance(transform.position, GetCurrentFloor());
            return distanceToPoint < elevatorTolerance;
        }

        void CycleFloor()
        {
            currentFloorIndex = elevatorPath.GetNextIndex(currentFloorIndex);
        }

        Vector3 GetCurrentFloor()
        {
            //Tweak elevator position at index(0)
            if(currentFloorIndex == 0 && !isPlatform)
            {
                Vector3 correctingElevator = elevatorPath.GetFloor(currentFloorIndex) + (Vector3.down * 2);
                return correctingElevator;
            }
            return elevatorPath.GetFloor(currentFloorIndex);
        }

        //If elevator is called allow to get floor index
        Vector3 GetCurrentFloor(int floorIndex)
        {
            return elevatorPath.GetFloor(floorIndex);
        }
    }
}
