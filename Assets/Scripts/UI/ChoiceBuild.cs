using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBuild : MonoBehaviour
{
    public GameObject ChoiceBuildObject;

    private GameObject Player;
    void Start()
    {
        Player = GameState.GetPlayer();
        GetComponent<Button>().onClick.AddListener(_ChoiceBuild);
    }

    void _ChoiceBuild() {
        if (!ChoiceBuildObject) {
            Debug.Log("Не установлен объект постройки!");
            return;
        }

        Player?.GetComponent<PlayerInteraction>().RepickBuild(ChoiceBuildObject);
        UIController.Controller.OpenOrCloseBuildPanel();
        Debug.Log("Постройка выбрана: " + ChoiceBuildObject.name);
    }
}
