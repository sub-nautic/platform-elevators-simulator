using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace Project.DestructableElements
{
    public class DestructableBody : MonoBehaviour
    {
        [SerializeField] BodyPart CoreBodyPart = null;
        [SerializeField] BodyPart leftFoot = null;
        [SerializeField] BodyPart rightFoot = null;
        [SerializeField] BodyPart leftFist = null;
        [SerializeField] BodyPart rightFist = null;
        [SerializeField] float oneLegSpeed = 5.6f;
        [SerializeField] float timeToResurectBodyPart = 5f;
        [SerializeField] ParticleSystem noLegVFX;

        bool isLeftFootDestroyed;
        bool isRightFootDestroyed;
        bool isLeftFistDestroyed;
        bool isRightFistDestroyed;

        float defaultMaxSpeed;

        private void Start()
        {
            defaultMaxSpeed = GetComponent<Mover>().GetMaxSpeed();
        }

        private void Update()
        {
            FlyForceIfHasNoFoots();


            if (CoreBodyPart.DestroyedStatus() == true)
            {
                GetComponent<Mover>().SetMaxSpeed(0f);
                GetComponent<Fighter>().CanFight(false);
                return;
            }

            CheckFistsStatus();
            CheckFootsStatus();
        }

        public float GetTimeToResurectBodyPart()
        {
            return timeToResurectBodyPart;
        }

        void CheckFistsStatus()
        {
            isLeftFistDestroyed = leftFist.DestroyedStatus();
            isRightFistDestroyed = rightFist.DestroyedStatus();

            if (isLeftFistDestroyed && isRightFistDestroyed)
            {
                GetComponent<Fighter>().CanFight(false);
            }
            else
            {
                GetComponent<Fighter>().CanFight(true);
            }
        }

        void CheckFootsStatus()
        {
            isLeftFootDestroyed = leftFoot.DestroyedStatus();
            isRightFootDestroyed = rightFoot.DestroyedStatus();

            InreaseSpeedIfHasOneLeg();
        }

        void InreaseSpeedIfHasOneLeg()
        {
            if (isLeftFootDestroyed || isRightFootDestroyed)
            {
                GetComponent<Mover>().SetMaxSpeed(oneLegSpeed);
            }
            else
            {
                GetComponent<Mover>().SetMaxSpeed(defaultMaxSpeed);
            }
        }

        void FlyForceIfHasNoFoots()
        {
            if (isLeftFootDestroyed && isRightFootDestroyed && !CoreBodyPart.DestroyedStatus())
            {
                noLegVFX.Play();
            }
            else
            {
                noLegVFX.Stop();
            }
        }
    }

}