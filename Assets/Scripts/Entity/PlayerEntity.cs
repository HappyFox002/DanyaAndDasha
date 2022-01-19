using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    public static PlayerEntity Player;

    protected override void EntityAlwaysUpdate()
    {

    }

    private void Awake()
    {
        Player = this;
        this.Dead += DeadHero;
    }

    private void DeadHero() {
        UIController.Controller.GameOver();
    }
}
