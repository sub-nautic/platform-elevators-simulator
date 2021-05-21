using Project.Control;
using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorCaller : MonoBehaviour, IRaycastable
    {
        [SerializeField] ElevatorController elevatorController;
        [SerializeField] int destinationFloor;

        bool called = false;

        public int Floor { get { return destinationFloor; } }

        void CallElevator()
        {
            called = true;
            elevatorController.MoveToPlayerSelectedFloor(destinationFloor);
            elevatorController.IsElevatorCalled(called);
            elevatorController.GetComponent<AudioPlayer>().ResetDiffAudio();
            called = false;
        }

        public bool HandleRaycast(Control.PlayerCamera callingController)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CallElevator();
            }
            return true;
        }
    }
}