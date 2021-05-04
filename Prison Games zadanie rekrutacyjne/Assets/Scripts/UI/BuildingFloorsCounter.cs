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

    PlayerMoverRB player;
    float atFloor;

    float thirdFloor;
    float secondFloor;
    float firstFloor;

    bool isPlayerInBuilding = false;

    private void Start()
    {
        thirdFloor = floors.GetFloor(3).y;
        secondFloor = floors.GetFloor(2).y;
        firstFloor = floors.GetFloor(1).y;
    }
    
    void Update()
    {
        PlayerInBuilding();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerInBuilding = true;
            player = other.gameObject.GetComponent<PlayerMoverRB>();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerInBuilding = false;
            floorTextToDisplay.text = "ground floor".ToString();
        }
    }

    void PlayerInBuilding()
    {
        if(!isPlayerInBuilding) return;

        atFloor = player.transform.position.y;

        OnWithFloor();

        if(OnWithFloor() == 0)
        {
            floorTextToDisplay.text = "ground floor".ToString();
            return;
        }
        floorTextToDisplay.text = OnWithFloor().ToString();      
    }

    int OnWithFloor()
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
