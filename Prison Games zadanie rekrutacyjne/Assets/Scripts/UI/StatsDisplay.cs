using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Project.Control;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeCounterText;
    [SerializeField] TextMeshProUGUI jumpCounterText;
    
    PlayerMover player;
    
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
        TimeCounter();
    }

    public void AddJumpCounter()
    {
        jumps += 1;

        jumpCounterText.text = jumps.ToString();
    }

    void TimeCounter()
    {
        timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        timeCounterText.text = string.Format(string.Format("{0}:{1}", minutes, seconds));
    }

}
