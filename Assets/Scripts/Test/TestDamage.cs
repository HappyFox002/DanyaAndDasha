using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<Entity>()?.RemoveHealth(5f);
    }
}
