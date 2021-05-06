using Project.Control;
using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorPlayerCheck : MonoBehaviour
    {
        Transform playerTransformParent;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                playerTransformParent = other.transform.parent;
                other.transform.parent = transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.transform.parent = playerTransformParent;
            }
        }
    }
}