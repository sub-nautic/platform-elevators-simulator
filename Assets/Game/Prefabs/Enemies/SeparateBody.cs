using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;

public class SeparateBody : MonoBehaviour
{

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

    [SerializeField] BodyPart CoreBodyPart = null;
    [SerializeField] BodyPart leftFoot = null;
    [SerializeField] BodyPart rightFoot = null;
    [SerializeField] float oneLegSpeed = 5.6f;

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
            GetComponent<Health>().Die();
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
        if (isLeftFootDestroyed && isRightFootDestroyed)
        {
            GetComponentInChildren<BodyPart>().DamageBodyPart();
        }
    }
}
