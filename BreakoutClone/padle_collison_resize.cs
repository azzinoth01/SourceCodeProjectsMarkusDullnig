using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class padle_collison_resize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform r=  transform.GetComponent<RectTransform>();

        BoxCollider2D colider=  gameObject.GetComponent<BoxCollider2D>();

        colider.size = new Vector2 (r.rect.width,r.rect.height);
    }
}
