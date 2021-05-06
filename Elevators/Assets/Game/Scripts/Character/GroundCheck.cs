using UnityEngine;

namespace Project.Control
{
    public class GroundCheck : MonoBehaviour
    {
        private bool detection = false;
        [SerializeField] private Material detectedMaterial;
        [SerializeField] private Material undetectedMaterial;
        private MeshRenderer rend;
        
        void Start()
        {
            rend = GetComponent<MeshRenderer>();
        }

        public void Detect(bool raycastResult)
        {
            detection = raycastResult;
            if (detection)
            {
                rend.material = detectedMaterial;
            }
            else
            {
                rend.material = undetectedMaterial;
            }
        }

        public bool IsDetected()
        {
            return detection;
        }
    }
}