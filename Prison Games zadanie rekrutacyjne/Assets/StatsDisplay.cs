using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Project.Control;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeCounterText;
    [SerializeField] TextMeshProUGUI jumpCounterText;
    [SerializeField] TextMeshProUGUI currentFloorText;
    
    PlayerMover player;

    float atFloor;
    float timer;
    int jumps = 0;
    
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
    }

    private void Start()
    {
        jumpCounterText.text = jumps.ToString();
    }

    void Update()
    {
        CurrentFloor();
        TimeCounter();

        atFloor = player.transform.position.y;
        currentFloorText.text = CurrentFloor().ToString();
    }

    public void AddJumpCounter()
    {
        jumps += 1;

        jumpCounterText.text = jumps.ToString();
    }

    int CurrentFloor()
    {
        switch (atFloor)
        {
            case float d when d >= 18.9:
                return 3;

            case float d when d >= 12.1:
                return 2;

            case float d when d >= 6.1:
                return 1;

            default:
                return 0;
        }
    }

    void TimeCounter()
    {
        timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        timeCounterText.text = string.Format(string.Format("{0}:{1}", minutes, seconds));
    }

}
