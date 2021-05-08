using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] myMeshs = null;
    [SerializeField] GameObject replacer;
    [SerializeField] BodyPart[] linkedParts = null;

    bool isDestroyed = false;
    public bool Status() { return isDestroyed; }

    public void DamageBodyPart()
    {
        DestroyMyself();
        CheckChildParts();

        print(this.name + "hit");

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

        Collider thisCollider = GetComponent<Collider>();

        thisCollider.enabled = false;
        foreach (SkinnedMeshRenderer myMesh in myMeshs)
        {
            myMesh.enabled = false;
        }

        GameObject disconectedPart = Instantiate(replacer, myMeshs[0].transform.position, Quaternion.identity);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
        float speed = 600;
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 1, force.z);
        disconectedPart.GetComponent<Rigidbody>().AddForce(force * speed);

        isDestroyed = true;
    }
}
