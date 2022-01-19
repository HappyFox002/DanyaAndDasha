using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInteraction : MonoBehaviour
{
    /// <summary>
    /// Префаб маркера постройки
    /// </summary>
    [SerializeField]
    private GameObject BuildObject;

    /// <summary>
    /// Созданный Маркер строительства
    /// </summary>
    [SerializeField]
    private GameObject BuildMarker;

    /// <summary>
    /// Длина взаимодействия с объектами
    /// </summary>
    [SerializeField]
    private float Distance = 10.0f;

    /// <summary>
    /// Флаг постройки
    /// </summary>
    private bool YouCanBuild = false;
    /// <summary>
    /// Построить объект
    /// </summary>
    private bool BuildingUp = false;

    /// <summary>
    /// Описание состояний персонажа
    /// </summary>
    public enum PlayerCondition { 
        Normal = 0,
        Build
    }

    /// <summary>
    /// Текущее состояние персонажа
    /// </summary>
    [SerializeField]
    private PlayerCondition CurrentCondition;

    void Start()
    {
        CurrentCondition = PlayerCondition.Normal;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!GameState.IsPaused)
                UIController.Controller.PausedGame();
            else
                UIController.Controller.ResumeGame();
        }

        if (GameState.IsPaused)
            return;

        if (Input.GetKeyDown(KeyCode.F)) {
            BuildObject = null;
            if (!BuildMarker)
                UIController.Controller.OpenOrCloseBuildPanel();
            SwitchMode();
        }

        if (Input.GetMouseButtonDown(0) && CurrentCondition == PlayerCondition.Build)
        {
            BuildingUp = true;
        }

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Distance, 1, QueryTriggerInteraction.Ignore)) {
            ActionVariant(hit.transform.gameObject, hit.point);
        }
    }

    /// <summary>
    /// Переключение режимов состояния
    /// </summary>
    void SwitchMode() {
        switch (CurrentCondition) {
            case PlayerCondition.Normal:
                {
                    CurrentCondition = PlayerCondition.Build;
                }
                break;
            case PlayerCondition.Build:
                {
                    CurrentCondition = PlayerCondition.Normal;
                    DestroyBuildMarker();
                }
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lookingObject">Объект слежения игрока</param>
    /// <param name="lookingPoint">Точка слежения персонажа</param>
    void ActionVariant(GameObject lookingObject, Vector3 lookingPoint) {
        switch (CurrentCondition) {
            case PlayerCondition.Normal: 
                    ActionNormal(lookingObject);
                break;
            case PlayerCondition.Build:
                    ActionBuild(lookingObject, lookingPoint);
                break;
        }
    }

    /// <summary>
    /// Уничтожение маркера объекта
    /// </summary>
    void DestroyBuildMarker() {
        if (!BuildMarker)
            return;

        var canBuild = BuildMarker.GetComponent<CanBuild>();
        canBuild.OnCanBuild -= CanBuild;
        canBuild.OnNotBuild -= NotBuild;
        Destroy(BuildMarker);
    }

    /// <summary>
    /// Взаимодействие персонажа с объектами
    /// </summary>
    /// <param name="lookingObject">Объект слежения игрока</param>
    void ActionNormal(GameObject lookingObject) {
        //Debug.Log($"Вы смотрите на {lookingObject.name}");
    }

    /// <summary>
    /// Взаимодействие персонажа при строительстве
    /// </summary>
    /// <param name="lookingObject">Объект слежения игрока</param>
    /// <param name="lookingPoint">Точка слежения персонажа</param>
    void ActionBuild(GameObject lookingObject, Vector3 lookingPoint) {
        ///Передвижение маркера
        if (BuildObject && BuildMarker) {
            if (TypeObjects.isTypeObject(TypeObject.Foundation, lookingObject))
            {
                Vector3 pos = VariantPositionFoundation(lookingObject.transform.position, lookingPoint);
                if (pos != Vector3.zero) {
                    BuildMarker.transform.position = pos;
                }
            }
            else
                BuildMarker.transform.position = lookingPoint;
        }
        ///Создание маркера объекта
        if (!BuildMarker && BuildObject) {
            BuildMarker = Instantiate(BuildObject, lookingPoint, BuildObject.transform.rotation);

            BuildMarker.AddComponent<CanBuild>();
            BuildMarker.GetComponent<BoxCollider>().isTrigger = true;
            if (!BuildMarker.GetComponent<Rigidbody>())
            {
                var rbBuildMarker = BuildMarker.AddComponent<Rigidbody>();
                rbBuildMarker.isKinematic = true;
                rbBuildMarker.useGravity = false;
            }

            var canBuild = BuildMarker.GetComponent<CanBuild>();
            canBuild.OnCanBuild += CanBuild;
            canBuild.OnNotBuild += NotBuild;
        }
        ///Отменить строительство
        if (BuildingUp && !YouCanBuild) {
            BuildingUp = false;
        }
        ///Построить объект
        if (BuildingUp && YouCanBuild)
        {
            Instantiate(BuildObject, BuildMarker.transform.position, BuildMarker.transform.rotation);
            BuildingUp = false;
            YouCanBuild = false;
        }
    }

    /// <summary>
    /// Разрешение на строительство объекта
    /// </summary>
    void CanBuild() {
        var meshMarker = BuildMarker.GetComponent<MeshRenderer>();
        foreach (var material in meshMarker.materials)
        {
            material.shader = Shader.Find("Transparent/Diffuse");
            material.color = new Color(66 / 100, 170 / 100, 255 / 100) * 0.5f;
        }
        YouCanBuild = true;
    }

    /// <summary>
    /// Запрет на строительство объекта
    /// </summary>
    void NotBuild() {
        var meshMarker = BuildMarker.GetComponent<MeshRenderer>();

        foreach (var material in meshMarker.materials)
        {
            material.shader = Shader.Find("Transparent/Diffuse");
            material.color = Color.red * 0.5f;
        }
        YouCanBuild = false;
    }

    public void RepickBuild(GameObject build) {
        YouCanBuild = false;
        BuildObject = build;
        DestroyBuildMarker();
    }

    public Vector3 VariantPositionFoundation(Vector3 obj, Vector3 point) {
        Vector3 positionCreate = Vector3.zero;
        Vector3 axis = obj - point;
        if (Mathf.Abs(axis.x) > Mathf.Abs(axis.z))
        {
            positionCreate = obj;
            positionCreate -= new Vector3(axis.x * 2,0,0);
        }
        else 
        {
            positionCreate = obj;
            positionCreate -= new Vector3(0, 0, axis.z * 2);
        }
        //Debug.Log($"Объект: {obj}, Точка: {point}, Result: {obj - point}, Position: {positionCreate}");

        return positionCreate;
    }
}
