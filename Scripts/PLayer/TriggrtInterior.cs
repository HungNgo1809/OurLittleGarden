using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
public class TriggrtInterior : MonoBehaviour
{
    public DataManager dataManager;
    public enum type
    {
        bed,
        sofa,
        arcade,
        armChair,
        chair,
        chess,
        desk,
        dinnerTable,
        drum,
        fridge,
        lamp,
        smallCarpet,
        tv,
        whiteCarpet,
        yellowCarpet
    }
    public type interiorType;
    public int Time = 7;
    private float timeDiff = 0;
    bool isTrigger;
    bool isSkipTime = false;
    bool isSlepped;
    GameObject player;

    public float changeDataSpeed;

    float afterSleep;
    float afterSleepFood;

    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PhotonNetwork.IsMasterClient)
        {
            isTrigger = true;
            player = other.gameObject;

            ActiveInteractUi(other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && PhotonNetwork.IsMasterClient)
        {
            isTrigger = false;
            ActiveInteractUi(other, false);
        }
    }
    public void ActiveInteractUi(Collider other, bool active)
    {
        if (other.gameObject.GetComponentInChildren<playerFloatingUi>() != null)
        {
            other.gameObject.GetComponentInChildren<playerFloatingUi>().transform.GetChild(0).gameObject.SetActive(active);
            PlayerControl control = other.gameObject.GetComponent<PlayerControl>();
            control.bubbleAnimation.Play("startBubble");

        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isTrigger)
        {
            if(Input.GetButtonDown("Use"))
            {
                switch(interiorType)
                {
                    case (type.bed):
                        if (!isSlepped)
                        {
                            if ((dataManager.sleep < 50.0f))
                            {
                                Sleep(100.0f);
                            }
                            else
                            {
                                //thông báo chưa buồn ngủ
                            }
                        }
                        else
                        {
                           
                        }
                        break;
                    case (type.sofa):
                        if (!isSlepped)
                        {
                            if ((dataManager.sleep < 50.0f))
                            {
                                Sleep(50.0f);
                            }
                            else
                            {
                                //thông báo chưa buồn ngủ
                            }
                        }
                        else
                        {

                        }
                        break;
                }
            }    
        }
        if (isSkipTime)
        {
            ChangeDataSleep();
            
            timeDiff -= 0.5f;

            if (Mathf.Round(timeDiff) == timeDiff) // Check if timeDiff is an integer
            {
                TimeManager.Instance.Tick();
            }

            if (timeDiff <= 0)
            {
                isSkipTime = false;
                isSlepped = false;
                WakeUp();
            }
        }
    }
    public void Sleep(float sleepValue)
    {
        afterSleep = dataManager.sleep + sleepValue;
        afterSleepFood = dataManager.food - 30.0f;

        isSlepped = true;
        // Set vị trí player và chạy anim nằm
        player.SetActive(false);
        player.transform.position = transform.position + offset;
        player.transform.rotation = Quaternion.Euler(0, transform.rotation.y - 90.0f, 0);
        player.SetActive(true);

        if (player.GetComponent<Animator>() != null)
        {
            player.GetComponent<Animator>().Play("sleep");
            PlayerControl control = player.GetComponent<PlayerControl>();
            control.NPCInteract_ = true;
        }
        QuestManager.Instance.CheckQuestRequirementSleep();
        QuestManager.Instance.CheckSideQuestRequirementSleep();
        //Skip time
        SleepSkipTime();
    }
    public void ChangeDataSleep()
    {
        dataManager.sleep = Mathf.Lerp(dataManager.sleep, afterSleep, changeDataSpeed);

        dataManager.food = Mathf.Lerp(dataManager.food, afterSleepFood, changeDataSpeed);
    }    
    public void WakeUp()
    {
        if (player.GetComponent<Animator>() != null)
        {
            player.GetComponent<Animator>().Play("Idle");
            PlayerControl control = player.GetComponent<PlayerControl>();
            control.NPCInteract_ = false;
        }
      
    }
    public void SleepSkipTime()
    {
       
        isSkipTime = true;
        int nextHour = TimeManager.Instance.timestamp.hour + Time - 24;
        int rightTime = TimeManager.Instance.timestamp.hour + Time;
      
        GameTimestamp nextTimestamp;

        if (nextHour >= 0)
        {
            // Skip to the next day
            int nextDay = TimeManager.Instance.timestamp.day + 1;
            nextTimestamp = new GameTimestamp(TimeManager.Instance.timestamp.year,
                                              TimeManager.Instance.timestamp.season,
                                              nextDay,
                                              nextHour,
                                              TimeManager.Instance.timestamp.minute);
        }
        else 
        {
            // Stay within the same day
            nextTimestamp = new GameTimestamp(TimeManager.Instance.timestamp.year,
                                              TimeManager.Instance.timestamp.season,
                                              TimeManager.Instance.timestamp.day,
                                              rightTime,
                                              TimeManager.Instance.timestamp.minute);
        }

        timeDiff = GameTimestamp.CompareTimestamps(nextTimestamp, TimeManager.Instance.timestamp);

     

    }
}
