using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance { get; set; }
    public DataManager dataManager;
    public HealthStaminaSlider slider;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(dataManager.coins <= 0)
        {
            dataManager.coins = 0;
        }    
        if(dataManager.stamina < 100)
        {
            dataManager.stamina = dataManager.stamina + 0.01f;
        }
        if (dataManager.stamina <= 0)
        {
            dataManager.stamina = 0f;
        }
        if (dataManager.food > 0 && dataManager.food <= slider.maxFood)
        {
            dataManager.food = dataManager.food - 0.0008f;
        }else if(dataManager.food <= 0)
        {
            if (dataManager.hp > 0)
            {
                dataManager.hp = dataManager.hp - 0.01f;
            }    

            dataManager.food = 0;
        }else if (dataManager.food > slider.maxFood)
        {
            dataManager.food = slider.maxFood;
        }

        if(dataManager.food >= 60)
        {
            dataManager.hp = dataManager.hp + 0.01f;
        }
        if (dataManager.hp <= 0)
        {
            dataManager.hp = 0;
            //chết rồi thì làm gì
        }
        if (dataManager.hp > 100)
        {
            dataManager.hp = 100;
        }

        if (dataManager.sleep > 0 && dataManager.sleep <= slider.maxSleep)
        {
            dataManager.sleep = dataManager.sleep - 0.00064f;
        }
        else if (dataManager.sleep <= 0)
        {
            if (dataManager.hp > 0)
            {
                dataManager.hp = dataManager.hp - 0.01f;
            }

            dataManager.sleep = 0;
        }
        else if (dataManager.sleep > slider.maxSleep)
        {
            dataManager.sleep = slider.maxSleep;
        }

        slider.SetFood(dataManager.food);
        slider.SetHP(dataManager.hp);
        slider.SetStamina(dataManager.stamina);
        slider.SetSleep(dataManager.sleep);
    }

    public void ChangeFood(float value)
    {
        dataManager.food = dataManager.food + value;
        slider.SetFood(dataManager.food);
    }
    public void ChangeHealth(float value)
    {
        dataManager.hp = dataManager.hp + value;
        slider.SetHP(dataManager.hp);
    }
    public void ChangeStamina(float value)
    {
        dataManager.stamina = dataManager.stamina + value;
        slider.SetStamina(dataManager.stamina);

    }
}
