using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Project.Elevators
{
    public class ElevatorButtonDispaly : MonoBehaviour
    {
        [SerializeField] TextMeshPro buttonText;

        void Start()
        {
            int floorOnButton = GetComponent<ElevatorCaller>().Floor;
            buttonText.text = floorOnButton.ToString();
        }
    }
}
