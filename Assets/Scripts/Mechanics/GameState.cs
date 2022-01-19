using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    /// <summary>
    /// Игровая пауза
    /// </summary>
    public static bool IsPaused {get; set;}

    public static GameObject GetPlayer() {
        return GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
        IsPaused = false;
    }

    void Update()
    {
        if (IsPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1f;
    }
}
