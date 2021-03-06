using System.Collections;
using System.Collections.Generic;
using Project.Control;
using Project.Elevators;
using TMPro;
using UnityEngine;

public class BuildingFloorsCounter : MonoBehaviour
{
    [Tooltip("Get current building elevator path object or if there is no elevator create new GameObject with needed script and set by creteing new child objects setting their transform on which floor schould be")]
    [SerializeField] ElevatorPath floors = null;
    [Tooltip("UI whitch display player current floor number")]
    [SerializeField] TextMeshProUGUI floorTextToDisplay;

    PlayerMover player;
    float atFloor;
    float thirdFloor, secondFloor, firstFloor;

    bool isPlayerInBuilding = false;

    private void Start()
    {
        thirdFloor = floors.GetFloor(3).y;
        secondFloor = floors.GetFloor(2).y;
        firstFloor = floors.GetFloor(1).y;
    }

    void Update()
    {
        HandlePlayerInBuilding();
    }

    void HandlePlayerInBuilding()
    {
        if (!isPlayerInBuilding) return;

        atFloor = player.transform.position.y;
        DisplayFloorText();
    }

    private void DisplayFloorText()
    {
        if (GetCurrentFloor() == 0)
        {
            DisplayGroundFloorText();
            return;
        }
        floorTextToDisplay.text = GetCurrentFloor().ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!HasPlayerTag(other)) return;
        isPlayerInBuilding = true;

        player = other.gameObject.GetComponent<PlayerMover>();
    }

    void OnTriggerExit(Collider other)
    {
        if (!HasPlayerTag(other)) return;
        isPlayerInBuilding = false;
        DisplayGroundFloorText();
    }

    bool HasPlayerTag(Collider other)
    {
        return other.gameObject.tag == "Player";
    }

    void DisplayGroundFloorText()
    {
        floorTextToDisplay.text = "ground floor".ToString();
    }

    int GetCurrentFloor()
    {
        switch (atFloor)
        {
            case float d when d >= thirdFloor:
                return 3;

            case float d when d >= secondFloor:
                return 2;

            case float d when d >= firstFloor:
                return 1;

            default:
                return 0;
        }
    }
}
