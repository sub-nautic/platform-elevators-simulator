﻿using System.Collections;
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
            calledElevatorDestination = GetCurrentFloor(testFloor) - Vector3.up;

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

        void CycleFloor()
        {
            currentFloorIndex = elevatorPath.GetNextIndex(currentFloorIndex);
        }

        Vector3 GetCurrentFloor()
        {
            if(currentFloorIndex == 0)
            {
                Vector3 nowy = elevatorPath.GetFloor(currentFloorIndex) + (Vector3.down * 2);
                return nowy;
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
