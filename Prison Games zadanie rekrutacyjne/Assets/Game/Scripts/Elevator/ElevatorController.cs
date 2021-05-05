using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorController : MonoBehaviour
    {
        [SerializeField] bool isPlatform;
        [SerializeField] float elevatorSpeedFraction = 0.01f;
        [SerializeField] float dwellingAtFloorTime = 1f;
        [SerializeField] float elevatorTolerance = 1f;
        [SerializeField] float waitForPlayerInteraction = 5f;
        [SerializeField] ElevatorPath elevatorPath;
        
        Vector3 elevatorPos;

        ElevatorMover elevatorMover;
        Vector3 calledElevatorDestination;

        float timeSinceArrivedAtFloor = Mathf.Infinity;
        int currentFloorIndex = 0;
        bool isElevatorCalled = false;
        //todo
        //bool elevatorArrived = false;

        int selectedDestination;
        
        void Awake()
        {
            elevatorMover = GetComponent<ElevatorMover>();

            elevatorPos = GetElevatorPos();
        }

        void Update()
        {
            if(isElevatorCalled)
            {
                playerCall();
            }
            else
            {
                ElevatorBehaviour();
            }

            //print("Is elevator called: " + isElevatorCalled);
            timeSinceArrivedAtFloor += Time.deltaTime;
        }
        
        void playerCall()
        {
            SelectFloor(selectedDestination, isElevatorCalled);
        }

        public void SelectFloor(int floorIndex, bool called)
        {
            selectedDestination = floorIndex;
            isElevatorCalled = called;
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
                //todo
                // if(AtFloor(calledElevatorDestination))
                // {
                //     elevatorArrived = true;
                //     StartCoroutine(ArrivedElevator());
                //     print("Check if elevator is on position");
                // }
            elevatorMover.StartMoveAction(calledElevatorDestination, elevatorSpeedFraction);
        }

        // IEnumerator ArrivedElevator()
        // {
        //     if(!elevatorArrived) yield break;
        //     yield return new WaitForSeconds(waitForPlayerInteraction);
        //     isElevatorCalled = false;
        // }

        Vector3 GetElevatorPos()
        {
            return transform.position;
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

        //todo
        //Checks if is on position of called elevator
        bool AtFloor(Vector3 distance)
        {
            float distanceToFloor = Vector3.Distance(transform.position, distance);
            return distanceToFloor < elevatorTolerance;
        }

        void CycleFloor()
        {
            currentFloorIndex = elevatorPath.GetNextIndex(currentFloorIndex);
        }

        Vector3 GetCurrentFloor()
        {
            //Correct elevator position at index(0)
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
