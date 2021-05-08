using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] myMeshs = null;
    [SerializeField] GameObject replacer;
    [SerializeField] BodyPart[] linkedParts = null;

    bool isDestroyed = false;

    public void DamageBodyPart()
    {
        Collider thisCollider = GetComponent<Collider>();

        print(this.name + "hit");
        thisCollider.enabled = false;
        foreach (SkinnedMeshRenderer myMesh in myMeshs)
        {
            myMesh.enabled = false;
        }
        Instantiate(replacer, myMeshs[0].transform.position, Quaternion.identity);

        isDestroyed = true;
    }
}
