using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_padle : MonoBehaviour
{
    private float countAngle;
    public float angle;
    private float lastYPos;
    public RectTransform panel;
    public RectTransform padle;

    // Start is called before the first frame update
    void Start()
    {
        padle = gameObject.GetComponent<RectTransform>();
        lastYPos = Input.mousePosition.y;
        countAngle = 0;
        angle = 0;
        globals.padle = gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        // block wenn alert screen pops up
        if(globals.AlertScreen == true) {
            return;
        }
        transform.localScale = globals.levelScale;

        if (Input.GetMouseButton(0)) {
            countAngle = countAngle + (Input.mousePosition.y - lastYPos);

           // Debug.Log("Diff Zähler");
            //Debug.Log(countAngle);

            angle = (countAngle / 5);

            if(angle > 90) {
                angle = 90;
                countAngle = 90 * 5;
            }
            else if(angle < -90) {
                angle = -90;
                countAngle = -90 * 5;
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);

        }
        if (Input.GetMouseButtonDown(1)) {
            // Debug.Log("The right mouse button was pressed");
            resetPos();
        }

        lastYPos = Input.mousePosition.y;

        //float oldY = transform.position.y;
        float oldLocalY = transform.localPosition.y;

        
        Vector3 v3 = Input.mousePosition;

       

        //Debug.Log("XMin");
        //Debug.Log(panel.rect.xMin);
        //Debug.Log("XMax");
        //Debug.Log(panel.rect.xMax);

       // Debug.Log("Paddle pos X before transform");
       // Debug.Log(v3.x);
        Camera cam = Camera.main;
        transform.position = cam.ScreenToWorldPoint(v3);
        //Debug.Log("Paddle postion y");
        //Debug.Log(oldLocalY);
        //Debug.Log("buttom panel");
        Vector3[] panelLocalCorners = new Vector3[4];
        
        panel.GetLocalCorners(panelLocalCorners);
        //Debug.Log(panelLocalCorners[0]);
     

        
        float leftMax;
        float rightMax;
        float height;
        float width;
        float padleWidth;

        padleWidth = ((padle.rect.width * padle.localScale.x) / 2);
        height = Mathf.Sin(angle * Mathf.Deg2Rad) * padleWidth;
        width = Mathf.Sqrt(((padleWidth * padleWidth) - (height * height)));

        leftMax = panel.rect.xMin + width;
        rightMax = panel.rect.xMax - width;

        float padleHeigth;
       
        float heightToButtom;
        float x;
        float maxAngel;

        padleHeigth = ((padle.rect.height * padle.localScale.y) / 2);

        // werte positiv machen damit die Rechung richtig ist
        heightToButtom = Mathf.Sqrt(panelLocalCorners[0].y * panelLocalCorners[0].y) - Mathf.Sqrt(padleHeigth * padleHeigth) - Mathf.Sqrt(oldLocalY * oldLocalY);

        

        if(padleWidth < heightToButtom) {
            maxAngel = 90;
        }
        else {

        
            x = Mathf.Sqrt((padleWidth * padleWidth) - (heightToButtom * heightToButtom));

            maxAngel = Mathf.Acos(((x / padleWidth))) * Mathf.Rad2Deg;
        }

        if (angle > maxAngel) {
            angle = maxAngel;
            countAngle = maxAngel * 5;
        }
        else if (angle < -maxAngel) {
            angle = -maxAngel;
            countAngle = -maxAngel * 5;
        }

        if (transform.localPosition.x < leftMax ){
            transform.localPosition = new Vector3(leftMax, oldLocalY, 0);
        }
        else if (transform.localPosition.x > rightMax) {
            transform.localPosition = new Vector3(rightMax, oldLocalY, 0);
        }
        else {        
            transform.localPosition= new Vector3(transform.localPosition.x, oldLocalY, 0);
        }
        // Debug.Log("Paddle position X");
        //  Debug.Log(transform.localPosition.x);

        //Debug.Log("Mouse Pos Y");
        //Debug.Log(Input.mousePosition.y);
        //Debug.Log("Last Pos Y");
        //Debug.Log(lastYPos);

       
    }

    public void resetPos() {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        countAngle = 0;
        angle = 0;
    }
}
