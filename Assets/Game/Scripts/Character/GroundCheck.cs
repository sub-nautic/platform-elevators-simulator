using UnityEngine;

namespace Project.Control
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] bool showGroundCheckStatus;
        [SerializeField] Material detectedMaterial;
        [SerializeField] Material undetectedMaterial;
        
        private MeshRenderer rend;
        
        private bool detection = false;
        
        void Start()
        {
            rend = GetComponent<MeshRenderer>();
        }

        public void Detect(bool raycastResult)
        {
            detection = raycastResult;
            if(showGroundCheckStatus)
            {
                if (detection)
                {
                    rend.material = detectedMaterial;
                }
                else
                {
                    rend.material = undetectedMaterial;
                }
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
        }

        public bool IsDetected()
        {
            return detection;
        }
    }
}