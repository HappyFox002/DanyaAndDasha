using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBuild : MonoBehaviour
{
    public delegate void IsCanBuild();
    public delegate void IsNotBuild();
    public event IsCanBuild OnCanBuild;
    public event IsNotBuild OnNotBuild;

    private void OnTriggerStay(Collider other)
    {
        if (TypeObjects.isTypeObject(TypeObject.Ground, other.gameObject))
        {
            OnCanBuild?.Invoke();
        }
        else
        {
            OnNotBuild?.Invoke();
        }
    }
}
