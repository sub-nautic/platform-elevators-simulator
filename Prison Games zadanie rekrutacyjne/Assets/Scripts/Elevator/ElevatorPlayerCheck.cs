using Project.Control;
using UnityEngine;

namespace Project.Elevators
{
    public class ElevatorPlayerCheck : MonoBehaviour
    {
        PlayerMover player = null;
        Vector3 platformVector;
        Transform playerTransformParent;
        bool detection = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                detection = true;
                player = other.GetComponent<PlayerMover>();
                //offset = player.transform.position - transform.position;
                playerTransformParent = other.transform.parent;
                other.transform.parent = transform;
                //print("wykryłem cię");
                player.IsOnPlatform(detection);
            }
        }

        // private void OnTriggerExit(Collider other)
        // {
            
        //     if (other.gameObject.tag == "Player")
        //     {
        //         //detection = false;
        //         other.transform.parent = playerTransformParent;
        //         //player.IsOnPlatform(detection);
        //         //player = null;
        //     }
        // }

        private void Update() 
        {
            if(detection)
            {
            }
                Vector3 platformVector = Vector3.zero;
                print(platformVector);
        }
    }
}