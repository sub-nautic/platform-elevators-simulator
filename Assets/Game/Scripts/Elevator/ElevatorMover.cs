using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorMover : MonoBehaviour
    {
        [Tooltip("Get transform of this GameObject")]
        [SerializeField] Transform elevatorPos;

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            elevatorPos.position = Vector3.MoveTowards(elevatorPos.position, destination, speedFraction * Time.deltaTime);
        }
    }
}