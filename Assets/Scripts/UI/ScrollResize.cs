using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollResize : MonoBehaviour
{
    private float scrollHeight = 0f;
    // Start is called before the first frame update
    void Start()
    {
        ResizeScroll();
    }

    public void ResizeScroll() {
        scrollHeight = 0f;
        foreach (Transform childUI in transform)
        {
            scrollHeight += childUI.GetComponent<RectTransform>().rect.height;
            //Debug.Log(childUI);
        }
        Debug.Log(scrollHeight);
        RectTransform content = transform.GetComponent<RectTransform>();
        content.sizeDelta = new Vector2(content.rect.width, scrollHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
