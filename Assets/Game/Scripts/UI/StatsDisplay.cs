using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    public static StatsDisplay instance;

    [SerializeField] TextMeshProUGUI timeCounterText;
    [SerializeField] public TextMeshProUGUI currentAmmoMagazine;
    [SerializeField] public TextMeshProUGUI allAmmo;

    float timer;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        TimeCounter();
    }

    public void Prints()
    {
        print("work");
    }

    void TimeCounter()
    {
        timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        timeCounterText.text = string.Format(string.Format("{0}:{1}", minutes, seconds));
    }

}
