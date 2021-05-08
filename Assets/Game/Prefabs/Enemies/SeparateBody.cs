using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] GameObject ex;

    private void Start()
    {
        Instantiate(ex, head.transform.position, Quaternion.identity);
    }
}
