using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeCounterText;

    float timer;

    private void Start()
    {

    }

    void Update()
    {
        TimeCounter();
    }

    void TimeCounter()
    {
        timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        timeCounterText.text = string.Format(string.Format("{0}:{1}", minutes, seconds));
    }

}
