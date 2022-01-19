using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider Bar;

    void Start()
    {
        Bar = GetComponent<Slider>();
        Bar.maxValue = PlayerEntity.Player.Health;
        PlayerEntity.Player.UpdateHealth += UpdateHealth;
    }

    void UpdateHealth(float hp)
    {
        Bar.value = hp;
    }
}
