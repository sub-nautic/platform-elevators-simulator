using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.DestructableElements
{
    public class SeparateBodyPart : MonoBehaviour
    {
        [SerializeField] ParticleSystem vfx = null;

        Vector3 currentPos;
        Vector3 BodyCurrentPos;
        float t;
        bool isSeparated = false;

        private void Start()
        {
            StartVFX();
        }

        private void Update()
        {
            if (isSeparated)
            {
                transform.position = Vector3.Lerp(currentPos, BodyCurrentPos, t);
            }
        }

        public void PartPos(Vector3 otherPos)
        {
            BodyCurrentPos = otherPos;
        }

        public IEnumerator MoveToPosition(float timeToMove)
        {
            currentPos = transform.position;
            t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / timeToMove;
                isSeparated = true;
                yield return null;
            }
            Destroy(gameObject);
        }

        private void StartVFX()
        {
            if (vfx == null) return;
            vfx.Play();
        }
    }
}
