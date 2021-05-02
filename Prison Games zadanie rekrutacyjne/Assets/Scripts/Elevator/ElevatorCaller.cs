using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorCaller : MonoBehaviour
    {
        [SerializeField] Transform calledFloor;
        [SerializeField] ElevatorController elevatorController;
        [SerializeField] int floor;

        bool called = false;
        
        public void CallElevator()
        {
            called = true;
            elevatorController.SelectFloor(floor, called);
            
            //???
            called = false;
        }
    }
}