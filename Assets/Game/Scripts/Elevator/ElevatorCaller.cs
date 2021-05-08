using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorCaller : MonoBehaviour
    {
        [SerializeField] ElevatorController elevatorController;
        [SerializeField] int floor;

        bool called = false;

        public int Floor { get { return floor; } }

        public void CallElevator()
        {
            called = true;
            elevatorController.MoveToPlayerSelectedFloor(floor);
            elevatorController.IsElevatorCalled(called);
            elevatorController.GetComponent<AudioPlayer>().ResetDiffAudio();
            called = false;
        }
    }
}