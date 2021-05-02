using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorController : MonoBehaviour
    {
        [SerializeField] float elevatorSpeedFraction = 0.01f;
        [SerializeField] float dwellingAtFloorTime = 1f;
        [SerializeField] float elevatorTolerance = 1f;
        [SerializeField] ElevatorPath elevatorPath;
        
        Vector3 elevatorPos;

        ElevatorMover elevatorMover;
        Vector3 calledElevatorDestination;

        float timeSinceArrivedAtFloor = Mathf.Infinity;
        int currentFloorIndex = 0;
        bool isElevatorCalled;

        int testFloor;
        
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

            print(isElevatorCalled);
            timeSinceArrivedAtFloor += Time.deltaTime;
        }

        void playerCall()
        {
            SelectFloor(testFloor, isElevatorCalled);
        }

        public void SelectFloor(int floorIndex, bool called)
        {
            testFloor = floorIndex;
            isElevatorCalled = called;
            calledElevatorDestination = GetSelectedFloor(testFloor);

            if (AtFloor(calledElevatorDestination))
            {
                return;
            }

            elevatorMover.StartMoveAction(calledElevatorDestination, elevatorSpeedFraction);
            print("Called");
        }

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
            float distanceToFloor = Vector3.Distance(transform.position, GetCurrentFloor());
            return distanceToFloor < elevatorTolerance;
        }
        bool AtFloor(Vector3 destination)
        {
            float distanceToFloor = Vector3.Distance(transform.position, destination);
            return distanceToFloor < elevatorTolerance;
        }

        void CycleFloor()
        {
            currentFloorIndex = elevatorPath.GetNextIndex(currentFloorIndex);
        }

        Vector3 GetCurrentFloor()
        {
            return elevatorPath.GetFloor(currentFloorIndex);
        }

        //If elevator is called allow to get floor index
        Vector3 GetSelectedFloor(int floorIndex)
        {
            return elevatorPath.GetFloor(floorIndex);
        }
    }
}
