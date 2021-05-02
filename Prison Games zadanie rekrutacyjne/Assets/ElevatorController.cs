using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorController : MonoBehaviour
    {
        [SerializeField] float elevatorSpeedFraction = 0.2f;
        [SerializeField] float dwellingAtFloorTime = 1f;
        [SerializeField] float elevatorTolerance = 1f;
        [SerializeField] ElevatorPath elevatorPath;
        
        Vector3 elevatorPos;

        ElevatorMover elevatorMover;

        float timeSinceArrivedAtFloor = Mathf.Infinity;
        int currentFloorIndex = 0;
        
        void Awake()
        {
            elevatorMover = GetComponent<ElevatorMover>();
            elevatorPos = GetElevatorPos();
        }

        void Update()
        {
            ElevatorBehaviour();            
            
            timeSinceArrivedAtFloor += Time.deltaTime;
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
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentFloor());
            return distanceToWaypoint < elevatorTolerance;
        }

        void CycleFloor()
        {
            currentFloorIndex = elevatorPath.GetNextIndex(currentFloorIndex);
        }

        Vector3 GetCurrentFloor()
        {
            return elevatorPath.GetFloor(currentFloorIndex);
        }
    }
}
