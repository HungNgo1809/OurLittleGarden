using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour, ITimeTracker
{
    public static UiManager Instance { get; set; }

    public Text timeText;
    public Text dateText;

    public GameObject caftPanel;
    // Start is called before the first frame update

    private void Start()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        TimeManager.Instance.RegisterTracker(this);
    }

    public void Active(GameObject obj)
    {
        if (!obj.activeSelf)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        string prefix = "AM ";

        if (hours > 12)
        {
            prefix = "PM ";
            hours -= 12;
        }
        timeText.text = prefix + hours + ":" + minutes.ToString("00");


        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfWeek = timestamp.GetDayOfTheWeek().ToString();

        dateText.text = season + " " + day + "(" + dayOfWeek + ")";
    }
    public void ActiveCraftPanel(bool active)
    {
        caftPanel.SetActive(active);
    }
}
