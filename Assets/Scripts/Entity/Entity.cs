using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected float health = 100;
    [SerializeField]
    protected float ElapsedTimeAttacked = 0.1f;

    public float Health { get { return health; } }

    public event _Dead Dead;
    public delegate void _Dead();

    public event _UpdateHealth UpdateHealth;
    public delegate void _UpdateHealth(float hp);

    private float lastTimeAttacked = 0f;
    private bool isAttacked = false;

    public void RemoveHealth(float value) {
        if (isAttacked)
            return;

        health -= value;
        UpdateHealth?.Invoke(health);

        isAttacked = true;
        lastTimeAttacked = 0;

        if (health <= 0)
            Dead?.Invoke();
    }

    public void AddHealth(float value) {
        health += value;
        UpdateHealth?.Invoke(health);
    }

    protected abstract void EntityAlwaysUpdate();

    private void Update()
    {
        EntityAlwaysUpdate();

        if (GameState.IsPaused)
            return;

        if (isAttacked)
            lastTimeAttacked += Time.deltaTime;
        if (lastTimeAttacked > ElapsedTimeAttacked)
            isAttacked = false;
    }
}
