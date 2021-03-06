using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace Project.DestructableElements
{
    public class BodyPart : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer[] myMeshs = null;
        [SerializeField] GameObject replacer;
        [SerializeField] BodyPart[] linkedParts = null;
        [SerializeField] float healthPoints = 15f;


        GameObject createdDisconectPart;
        SeparateBodyPart detachedPart;
        Vector3 currentPos;

        float resurrectTime = 5f;
        float startingHealthPoints;

        bool isDestroyed = false;
        public bool DestroyedStatus() { return isDestroyed; }

        void Start()
        {
            startingHealthPoints = healthPoints;
        }

        void Update()
        {
            MoveDetachedPartToOriginalPos();
        }

        public void DamageBodyPart(float gunDamage)
        {
            healthPoints -= gunDamage;
            GetComponentInParent<Health>().takeDamage.Invoke(gunDamage);
            if (healthPoints <= 0)
            {
                DestroyMyself();
                CheckChildParts();
            }
            //print(this.name + "hit");
        }

        void CheckChildParts()
        {
            foreach (BodyPart linkedPart in linkedParts)
            {
                linkedPart.DestroyMyself();
            }
        }

        void DestroyMyself()
        {
            if (isDestroyed) return;
            ColliderState(false);
            MeshState(false);
            CreateSeparatePart();

            float timeToStartingResurect = GetComponentInParent<DestructableBody>().GetTimeToResurectBodyPart();
            Invoke("BackToOryginalForm", timeToStartingResurect);

            isDestroyed = true;
        }

        private void CreateSeparatePart()
        {
            createdDisconectPart = Instantiate(replacer, myMeshs[0].transform.position, Quaternion.identity);

            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
            float speed = 600;
            Vector3 force = transform.forward;
            force = new Vector3(force.x, 1, force.z);
            createdDisconectPart.GetComponent<Rigidbody>().AddForce(force * speed);
        }

        private void ColliderState(bool condition)
        {
            Collider thisCollider = GetComponent<Collider>();
            thisCollider.enabled = condition;
        }

        private void MeshState(bool condition)
        {
            foreach (SkinnedMeshRenderer myMesh in myMeshs)
            {
                myMesh.enabled = condition;
            }
        }

        void BackToOryginalForm()
        {
            detachedPart = createdDisconectPart.GetComponent<SeparateBodyPart>();

            StartCoroutine(detachedPart.MoveToPosition(resurrectTime));
            StartCoroutine(Resurrect());
        }

        private void MoveDetachedPartToOriginalPos()
        {
            if (detachedPart == null) return;
            currentPos = transform.position;
            detachedPart.PartPos(currentPos);
        }

        IEnumerator Resurrect()
        {
            yield return new WaitForSeconds(resurrectTime);

            ColliderState(true);
            MeshState(true);
            healthPoints = startingHealthPoints;

            isDestroyed = false;
        }
    }
}