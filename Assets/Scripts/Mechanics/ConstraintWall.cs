using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintWall : MonoBehaviour
{
    private MeshRenderer render;
    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent)
            render = transform.parent.GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != GameState.GetPlayer())
            return;
        render.enabled = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != GameState.GetPlayer())
            return;
        render.enabled = false;
    }
}
