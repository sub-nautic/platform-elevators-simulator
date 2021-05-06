using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAtEddge : MonoBehaviour
{
    bool detectCol = true;
    
    // public void Detect(bool result)
    // {
    //     detectCol = result;
    // }

    public bool IsDetected()
    {
        return detectCol;
    }
    
    private void OnTriggerExit(Collider other)
    {
        detectCol = false;
    }
    private void OnTriggerStay(Collider other)
    {
        detectCol = true;
    }
}
