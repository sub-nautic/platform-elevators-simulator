using Project.Control;
using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorPlayerCheck : MonoBehaviour
    {
        Transform playerTransformParent;
        bool isPlayerIn = false;
        
        public bool IsPlayerIn { get { return isPlayerIn; } }
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                isPlayerIn = true;
                playerTransformParent = other.transform.parent;
                other.transform.parent = transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                isPlayerIn = false;
                other.transform.parent = playerTransformParent;
            }
        }
    }
}