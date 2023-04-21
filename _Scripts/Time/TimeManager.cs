using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Interal Clock")]
    [SerializeField]
    GameTimestamp timestamp;
    public float timeScale = 1.0f;

    [Header("Day and Night cycle")]
    public Transform sunTransform;


    public List<ITimeTracker> listeners = new List<ITimeTracker>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timestamp = new GameTimestamp(0, GameTimestamp.Season.Spring, 1, 12, 0);
        StartCoroutine(TimeUpdate());
    }

    // Update is called once per frame

    public GameTimestamp GetGameTimestamp()
    {
        return new GameTimestamp(timestamp);
    }
    IEnumerator TimeUpdate()
    {
        while (true)
        {

            Tick();
            yield return new WaitForSeconds(1 / timeScale);

        }

    }

    public void Tick()
    {
        timestamp.UpdateClock();

        foreach (ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }

        UpdateSunMovement();


    }

    void UpdateSunMovement()
    {
        // 360/24 = 15 do / 1h mat troi -> 15/60 = 0.25 do / 1 phut
        int timeInMinutes = GameTimestamp.HoursToMinutes(timestamp.hour) + timestamp.minute;

        float sunAngle = .25f * timeInMinutes - 90; // thoi diem mat troi lan la -90 do

        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }
}
