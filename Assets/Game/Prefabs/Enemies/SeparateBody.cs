using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateBody : MonoBehaviour
{
    [SerializeField] BodyPart leftFoot;
    [SerializeField] BodyPart rightFoot;

    [Header("Main parts")]
    [SerializeField] GameObject head = null;
    [SerializeField] GameObject chest = null;

    [Header("Right side parts")]
    [SerializeField] GameObject rHand = null;
    [SerializeField] GameObject rForearm = null;
    [SerializeField] GameObject rScholder = null;
    [SerializeField] GameObject rFoot = null;
    [SerializeField] GameObject rShin = null;
    [SerializeField] GameObject rThigh = null;

    [Header("Left side parts")]
    [SerializeField] GameObject lHand = null;
    [SerializeField] GameObject lForearm = null;
    [SerializeField] GameObject lScholder = null;
    [SerializeField] GameObject lFoot = null;
    [SerializeField] GameObject lShin = null;
    [SerializeField] GameObject lThigh = null;

    bool isLeftFootDestroyed;
    bool isRightFootDestroyed;

    private void Update()
    {
        CheckFootsStatus();
    }

    void CheckFootsStatus()
    {
        isLeftFootDestroyed = leftFoot.Status();
        isRightFootDestroyed = rightFoot.Status();

        if (isLeftFootDestroyed && isRightFootDestroyed)
        {
            GetComponentInChildren<BodyPart>().DamageBodyPart();
        }
    }
}
