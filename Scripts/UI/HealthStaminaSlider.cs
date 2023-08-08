using UnityEngine;
using UnityEngine.UI;

public class HealthStaminaSlider : MonoBehaviour
{
    public Slider hpSlider;
    public Slider staminaSlider;
    public Slider foodSlider;
    public Slider sleepSlider;

    public float maxHP = 100f;
    public float currentHP = 100f;
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float maxFood = 100f;
    public float currentFood = 100f;
    public float maxSleep = 100f;
    public float currentSleep = 100f;

    void Start()
    {
        // Set the maximum value of the HP and Stamina sliders
        hpSlider.maxValue = maxHP;
        staminaSlider.maxValue = maxStamina;
        foodSlider.maxValue = maxFood;
        sleepSlider.maxValue = maxSleep;
        // Set the initial value of the HP and Stamina sliders
        hpSlider.value = currentHP;
        staminaSlider.value = currentStamina;
        foodSlider.value = currentFood;
        sleepSlider.value = currentSleep;
    }

    public void SetHP(float value)
    {
        // Update the current HP value and the HP slider
        currentHP = value;
        hpSlider.value = currentHP ;
    }

    public void SetStamina(float value)
    {
        // Update the current Stamina value and the Stamina slider
        currentStamina = value;
        staminaSlider.value = currentStamina ;
    }

    public void SetFood(float value)
    {
        currentFood = value;
        foodSlider.value = currentFood ;
    }
    public void SetSleep(float value)
    {
        currentSleep = value;
        sleepSlider.value = currentSleep;
    }
}
