using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile_layer : MonoBehaviour
{
    public int currentLayer;
    public Sprite sp;
    private SpriteRenderer spriteRend;
    private bool visible;

    private void Awake() {
        gameObject.TryGetComponent<SpriteRenderer>(out spriteRend);
        visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentLayer = transform.parent.gameObject.GetComponent<paint_grid>().currentlayer;
        if(currentLayer < spriteRend.sortingOrder || visible == false) {

            spriteRend.sprite = null;
        }
        else {
            spriteRend.sprite = sp;
        }
    }

  

}
