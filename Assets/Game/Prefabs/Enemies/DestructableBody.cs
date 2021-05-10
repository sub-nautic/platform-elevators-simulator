using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;

namespace Project.DestructableElements
{
    public class DestructableBody : MonoBehaviour
    {
        [SerializeField] BodyPart CoreBodyPart = null;
        [SerializeField] BodyPart leftFoot = null;
        [SerializeField] BodyPart rightFoot = null;
        [SerializeField] float oneLegSpeed = 5.6f;
        [SerializeField] ParticleSystem noLegVFX;

        bool isLeftFootDestroyed;
        bool isRightFootDestroyed;

        private void Update()
        {
            CheckFootsStatus();
        }

        void CheckFootsStatus()
        {
            if (CoreBodyPart.Status() == true)
            {
                //GetComponent<Health>().Die();
            }

            isLeftFootDestroyed = leftFoot.Status();
            isRightFootDestroyed = rightFoot.Status();

            if (!isLeftFootDestroyed && !isRightFootDestroyed) return;

            DestroyIfHasNoFoots();
            InreaseSpeedIfHasOneLeg();
        }

        private void InreaseSpeedIfHasOneLeg()
        {
            if (isLeftFootDestroyed || isRightFootDestroyed)
            {
                GetComponent<Mover>().SetMaxSpeed(oneLegSpeed);
            }
        }

        private void DestroyIfHasNoFoots()
        {
            if (isLeftFootDestroyed && isRightFootDestroyed && !CoreBodyPart.Status())
            {
                noLegVFX.Play();
                //GetComponentInChildren<BodyPart>().DamageBodyPart();
            }
            else
            {
                noLegVFX.Stop();
            }
        }
    }

}