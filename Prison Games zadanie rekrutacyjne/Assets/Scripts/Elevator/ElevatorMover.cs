using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorMover : MonoBehaviour
    {
        //[SerializeField] float maxSpeed = 6f;
        
        Vector3 elevatorPos;
        
        private void Start()
        {
            elevatorPos = transform.position;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            elevatorPos = Vector3.Lerp(elevatorPos, destination, speedFraction);
            print("Move");
        }
    }
}