using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resize_padle : MonoBehaviour
{
    public RectTransform Panel;
    public RectTransform LeftPadle;
    public RectTransform RightPadle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scaleX;
        scaleX = Panel.rect.width - LeftPadle.rect.width - RightPadle.rect.width;
        scaleX = scaleX / gameObject.GetComponent<RectTransform>().rect.width;

        Vector3 scale = new Vector3(scaleX, 1, 1);
        gameObject.GetComponent<RectTransform>().localScale = scale;
    }
}
