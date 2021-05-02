using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorMover : MonoBehaviour
    {
        [SerializeField] Transform elevatorPos;        

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            elevatorPos.position = Vector3.Lerp(elevatorPos.position, destination, speedFraction);
            print("Move");
        }
    }
}