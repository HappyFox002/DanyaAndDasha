using System;
using UnityEngine;

// Типы объектов
[Flags]
public enum TypeObject { 
    None = 0,
    Ground = 1, 
    Trees = 2,
    SmallTrees = 4,
    Rocks = 8,
    Foundation = 16,
    Builds = 32,
    Other = 64
}

public class TypeObjects : MonoBehaviour {
    [SerializeField]
    private TypeObject GameType = TypeObject.None;

    public TypeObject GetGameType() {
        return GameType;
    }

    /// <summary>
    /// Проверка обладает ли объект нужным типом.
    /// </summary>
    /// <param name="type">Нужный тип</param>
    /// <param name="obj">Объект проверки</param>
    /// <returns></returns>
    public static bool isTypeObject(TypeObject type, GameObject obj) {
        if(obj == null)
            return false;

        TypeObjects checkObj = obj.GetComponent<TypeObjects>();

        if (checkObj == null)
            return false;

        if ((checkObj.GetGameType() & type) == type)
            return true;

        return false;
    }
}