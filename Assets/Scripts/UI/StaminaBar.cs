using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider Bar;

    void Start()
    {
        Bar = GetComponent<Slider>();
        PlayerController.WastageStamina += UpdateStamina;
    }

    void UpdateStamina(float stamina) {
        Bar.value = stamina;
    }
}
