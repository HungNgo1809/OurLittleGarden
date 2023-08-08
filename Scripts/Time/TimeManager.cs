using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering.PostProcessing;
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public DataManager dataManager;
    [Header("Interal Clock")]
   // [SerializeField]
    public GameTimestamp timestamp ;
    public GameTimestamp newTime = new GameTimestamp(0, 0, 0, 0, 0);
    public float timeScale = 1.0f;
    //public ColorTimeEffect colorTimeEffect;
    [Header("Day and Night cycle")]
    public Transform sunTransform;
    public Light sunColor;
    public PostProcessVolume postProcessVolume;
    public ParticleSystem fireFly;
    private ColorGrading colorGradingLayer;

    public List<ITimeTracker> listeners = new List<ITimeTracker>();
    private void Awake()
    {
        //DontDestroyOnLoad(this);
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
        
        if(UiManager.Instance.timeText.gameObject.activeSelf == false)
        {
            UiManager.Instance.timeText.gameObject.SetActive(true);

        }
        if (UiManager.Instance.dateText.gameObject.activeSelf == false)
        {
            UiManager.Instance.dateText.gameObject.SetActive(true);

        }

        if (dataManager.timeData.seasons == 0 )
        {
            timestamp = new GameTimestamp(dataManager.timeData.years, GameTimestamp.Season.Spring, dataManager.timeData.days, dataManager.timeData.hourrs, dataManager.timeData.mintute);
        }
        else if (dataManager.timeData.seasons == 1)
        {
            timestamp = new GameTimestamp(dataManager.timeData.years, GameTimestamp.Season.Summer, dataManager.timeData.days, dataManager.timeData.hourrs, dataManager.timeData.mintute);
        }
        else if (dataManager.timeData.seasons == 2)
        {
            timestamp = new GameTimestamp(dataManager.timeData.years, GameTimestamp.Season.Fall, dataManager.timeData.days, dataManager.timeData.hourrs, dataManager.timeData.mintute);
        }
        else if (dataManager.timeData.seasons == 3)
        {
            timestamp = new GameTimestamp(dataManager.timeData.years, GameTimestamp.Season.Winter, dataManager.timeData.days, dataManager.timeData.hourrs, dataManager.timeData.mintute);
        }


        Debug.Log(timestamp.season + "---"+ timestamp.year + "---" + timestamp.day + "---" + timestamp.hour + "---" + timestamp.minute);


        postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
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

        dataManager.timeData.years = timestamp.year;
        if (timestamp.season == GameTimestamp.Season.Spring)
        {
            dataManager.timeData.seasons = 0;
        }
        else if (timestamp.season == GameTimestamp.Season.Summer)
        {
            dataManager.timeData.seasons = 1;
        }
        else if (timestamp.season == GameTimestamp.Season.Fall)
        {
            dataManager.timeData.seasons = 2;
        }
        else if (timestamp.season == GameTimestamp.Season.Winter)
        {
            dataManager.timeData.seasons = 3;
        }
        dataManager.timeData.days = timestamp.day;
        dataManager.timeData.hourrs = timestamp.hour;
        dataManager.timeData.mintute = timestamp.minute;

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
   

        float sunAngle = 0.25f * timeInMinutes - 90;
        sunTransform.eulerAngles = new Vector3(50, sunAngle, 0);

        if (timeInMinutes <= 240 || timeInMinutes >= 1080)
        {
            float t = 0;

            float intensity = Mathf.Lerp(0, 1, t);

            sunTransform.GetComponent<Light>().intensity = intensity;
        }

        else if (timeInMinutes >= 480 || timeInMinutes <= 960)
        {
            float t = 1;

            float intensity = Mathf.Lerp(0, 1, t);

            sunTransform.GetComponent<Light>().intensity = intensity;
        }

        if (timeInMinutes >= 240 && timeInMinutes <= 480)
        {
            float t = ((float)timeInMinutes - 240) / 240;


            t = Mathf.Clamp01(t);

            float intensity = Mathf.Lerp(0, 1, t);
         
            sunTransform.GetComponent<Light>().intensity = intensity;
        }

        else if (timeInMinutes >= 960 && timeInMinutes <= 1080)
        {
            float t = ((float)timeInMinutes - 960) / 120;
            t = Mathf.Clamp01(t);

            float intensity = Mathf.Lerp(1, 0, t);

            sunTransform.GetComponent<Light>().intensity = intensity;
        }


        if (timeInMinutes < 420) // 0 - > 12h tu 0.13 toi 1 sua lai tu 18:00 toi 4:00 nghia la lerp tu 
        {
            /* // Lerp tu 960 toi 1080 tu 1 toi 0
            t = timeInMinutes / 720f;
            t = Mathf.Clamp01(t);

            float intensity = Mathf.Lerp(0.13f, 1f, t);
       
            sunTransform.GetComponent<Light>().intensity = intensity;

            */

            float t = (float)timeInMinutes / 420;
            t = Mathf.Clamp01(t);

            Color startColor = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255f / 255f);
            Color endColor = new Color(245 / 255f, 167 / 255f, 26 / 255f, 255f / 255f);

            Color lerpedColor = Color.Lerp(startColor , endColor, t);
            sunColor.color = lerpedColor;
        }
        else if(timeInMinutes >= 420 && timeInMinutes < 720)
        {
            /*
            t = (timeInMinutes - 720) / 720f; // 12 -> 24h tu 1 toi 0.13 sua lai tu 8:00 toi 16:00 sang nhat

            t = Mathf.Clamp01(t);   // lerp tu 240 toi 480 tu 0 toi 1
            float intensity = Mathf.Lerp(1,0.13f, t);

            sunTransform.GetComponent<Light>().intensity = intensity;
             */
            float t = ((float)timeInMinutes - 420) / 300;
          
            t = Mathf.Clamp01(t);  
            Color startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Color endColor = new Color(245 / 255f, 167 / 255f, 26 / 255f, 255f / 255f);

            Color lerpedColor = Color.Lerp(endColor,startColor, t);
            sunColor.color = lerpedColor;
        }
        else if (timeInMinutes >= 720 && timeInMinutes < 1020)
        {
            /*
            t = (timeInMinutes - 720) / 720f; // 12 -> 24h tu 1 toi 0.13 sua lai tu 8:00 toi 16:00 sang nhat

            t = Mathf.Clamp01(t);   // lerp tu 240 toi 480 tu 0 toi 1
            float intensity = Mathf.Lerp(1,0.13f, t);

            sunTransform.GetComponent<Light>().intensity = intensity;
             */
            float t = ((float)timeInMinutes - 720) / 300;

            t = Mathf.Clamp01(t);
            Color startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Color endColor = new Color(245 / 255f, 167 / 255f, 26 / 255f, 255f / 255f);

            Color lerpedColor = Color.Lerp(startColor , endColor, t);
            sunColor.color = lerpedColor;
        }
        else if (timeInMinutes >= 1020 && timeInMinutes < 1140)
        {
            float t = ((float)timeInMinutes - 1020) / 120;

            t = Mathf.Clamp01(t);
            Color startColor = new Color(245 / 255f, 167 / 255f, 26 / 255f, 255f / 255f);
            Color endColor = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255f / 255f);

            Color lerpedColor = Color.Lerp(startColor , endColor, t);
            sunColor.color = lerpedColor;
        }

        // FILTER
        //thieu timeInMinutes >= 18*60 + 30 thi phai = filter toi
        
      
        
        if (timeInMinutes < 5*60 ) //18h toi 18r filter hh -> filter toi
        {

            Color startColor = new Color(126 / 255f, 166 / 255f, 217 / 255f, 255f / 255f);

            colorGradingLayer.colorFilter.value = startColor;
         
        }
        else if (timeInMinutes > 18 * 60 + 30 && timeInMinutes <= 24*60) //18h toi 18r filter hh -> filter toi
        {

            Color startColor = new Color(126 / 255f, 166 / 255f, 217 / 255f, 255f / 255f);

            colorGradingLayer.colorFilter.value = startColor;

          

        }
        else if (timeInMinutes >= 6 * 60 && timeInMinutes < 7*60 + 30) //6->7rh filter trang full 
        {
            float t = ((float)timeInMinutes - 6*60) / 90;
            t = Mathf.Clamp01(t);

            Color startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Color endColor = new Color(255 / 255f, 190 / 255f, 162 / 255f, 255f / 255f);

            Color lerpedColor = Color.Lerp(endColor , startColor, t);
    
            colorGradingLayer.colorFilter.value = lerpedColor;

            
        }
        else if (timeInMinutes >= 7 * 60 + 30 && timeInMinutes < 9*60) //7r->9rh filter trang full 
        {
   

            Color startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);


            colorGradingLayer.colorFilter.value = startColor;

        }
        else if (timeInMinutes >= 9 * 60 && timeInMinutes < 11 * 60) //9h toi 11 h filter trang - > filer trua
        {
            float t = ((float)timeInMinutes - 9 * 60) / 120;
            t = Mathf.Clamp01(t);
         

            Color startColor = new Color(250 / 255f, 214 / 255f, 133 / 255f, 255f / 255f);
            Color endColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

            Color lerpedColor = Color.Lerp(endColor, startColor, t);
            
            colorGradingLayer.colorFilter.value = lerpedColor;

        }
        else if (timeInMinutes >= 11*60 && timeInMinutes < 14*60) //giu filter trua
        {
  

            Color startColor = new Color(250 / 255f, 214 / 255f, 133 / 255f, 255f / 255f);

            colorGradingLayer.colorFilter.value = startColor;

        }
        else if (timeInMinutes >= 14 * 60 && timeInMinutes < 16 * 60 )
        {
            float t = ((float)timeInMinutes - 14 * 60) / 120 ;
            t = Mathf.Clamp01(t);

            Color startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Color endColor = new Color(250 / 255f, 214 / 255f, 133 / 255f, 255f / 255f); //filter trua

            Color lerpedColor = Color.Lerp(endColor, startColor, t);

            colorGradingLayer.colorFilter.value = lerpedColor;

        }
        else if (timeInMinutes >= 16 * 60 && timeInMinutes < 17 * 60)
        {

            Color startColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

            colorGradingLayer.colorFilter.value = startColor;

        }

        else if (timeInMinutes >= 17 * 60 && timeInMinutes < 18*60) //fiulter trang toi hh
        {
            float t = ((float)timeInMinutes - 17 * 60) / 60;
            t = Mathf.Clamp01(t);

            Color startColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255f / 255f);
            Color endColor = new Color(255 / 255f, 190 / 255f, 162 / 255f, 255f / 255f);

            Color lerpedColor = Color.Lerp(startColor,  endColor, t);

            colorGradingLayer.colorFilter.value = lerpedColor;

        }
        else if (timeInMinutes >= 18 * 60 && timeInMinutes < 18 * 60 + 30) //18h toi 18r filter hh -> filter toi
        {
            float t = ((float)timeInMinutes - 18 * 60) / 30;
            t = Mathf.Clamp01(t);

            Color startColor = new Color(126 / 255f, 166 / 255f, 217 / 255f, 255f / 255f);
            Color endColor = new Color(255 / 255f, 190 / 255f, 162 / 255f, 255f / 255f);

            Color lerpedColor = Color.Lerp(endColor, startColor, t);

            colorGradingLayer.colorFilter.value = lerpedColor;

        }

        else if (timeInMinutes >= 5 * 60 && timeInMinutes < 5 * 60 + 30) //5h toi 5r filter toi -> filter binhminh
        {
            float t = ((float)timeInMinutes - 5 * 60) / 30;
            t = Mathf.Clamp01(t);

            Color startColor = new Color(255 / 255f, 190 / 255f, 162 / 255f, 255f / 255f);
            Color endColor = new Color(126 / 255f, 166 / 255f, 217 / 255f, 255f / 255f);

            Color lerpedColor = Color.Lerp(endColor, startColor, t);

            colorGradingLayer.colorFilter.value = lerpedColor;

        }
      

        if(timeInMinutes >= 18*60 || timeInMinutes < 4 * 60 + 30 && !fireFly.isPlaying)
        {
            fireFly.Play();
        }
        else if (timeInMinutes >= 4 * 60 + 30 && timeInMinutes < 18*60 && fireFly.isPlaying)
        {
            fireFly.Stop();
        }
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
