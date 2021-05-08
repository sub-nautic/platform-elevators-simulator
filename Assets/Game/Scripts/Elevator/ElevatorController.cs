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
        Vector3 calledElevatorDestination;
        ElevatorMover elevatorMover;
        AudioPlayer audioPlayer;
        float elevatorTolerance;
        int SelectedDestinationIndex;

        float timeSinceArrivedAtFloor = Mathf.Infinity;
        int currentFloorIndex = 0;
        bool isElevatorCalled = false;

        void Awake()
        {
            elevatorMover = GetComponent<ElevatorMover>();
            audioPlayer = GetComponent<AudioPlayer>();

            elevatorPos = transform.position;
        }

        void Start()
        {
            elevatorTolerance = isPlatform ? 0.0001f : 1f;
        }

        void Update()
        {
            if (isElevatorCalled)
            {
                MoveToPlayerSelectedFloor(SelectedDestinationIndex);
            }
            else
            {
                ElevatorBehaviour();
            }
            //print(gameObject.name +" called: " + isElevatorCalled);

            timeSinceArrivedAtFloor += Time.deltaTime;
        }

        public bool IsElevatorCalled(bool isCalled) { return isElevatorCalled = isCalled; }
        public void MoveToPlayerSelectedFloor(int floorIndex)
        {
            SelectedDestinationIndex = floorIndex;
            if (!isPlatform)
            {
                calledElevatorDestination = GetCurrentFloor(SelectedDestinationIndex) - Vector3.up;
            }
            else
            {
                calledElevatorDestination = GetCurrentFloor(SelectedDestinationIndex);
            }
            elevatorMover.StartMoveAction(calledElevatorDestination, elevatorSpeedFraction);
            Vector3 newPosition = calledElevatorDestination;

            //When elevator will be on destination floor time will start counting
            if (newPosition != transform.position) { timeSinceArrivedAtFloor = 0; }
            audioPlayer.PlayDefaultAudio();
            audioPlayer.PlayDiffrentAudio(0);

            //If in destination position            
            if (newPosition == transform.position)
            {
                audioPlayer.StopAudio();
                if (timeSinceArrivedAtFloor > waitInIdleState)
                {
                    isElevatorCalled = false;
                    audioPlayer.PlayDiffrentAudio(0);
                }
                //print(gameObject.name + " - Im at destination point");
            }
        }

        void ElevatorBehaviour()
        {
            Vector3 nextPosition = elevatorPos;

            if (elevatorPath != null)
            {
                if (IsAtFloor())
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
            audioPlayer.PlayDefaultAudio();
        }

        bool IsAtFloor()
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
            if (currentFloorIndex == 0 && !isPlatform)
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
