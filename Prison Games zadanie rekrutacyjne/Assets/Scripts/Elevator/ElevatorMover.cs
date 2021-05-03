using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorMover : MonoBehaviour
    {
        [SerializeField] Transform elevatorPos;
        float speed = 5;

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            //elevatorPos.position = Vector3.Lerp(elevatorPos.position, destination, speedFraction);
            //elevatorPos.position = destination;
            elevatorPos.position = Vector3.MoveTowards(elevatorPos.position, destination, speed * Time.deltaTime);
            print("Move");
        }
    }
}