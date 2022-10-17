using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class powerUpMove : MonoBehaviour
{
    public float speed;
    private int stop;
    private int noStop;
    private float  waitTime;

    //public Grid grid;
    //private float currentScreenWidth;
    //private float currentScreenHeight;
    //  private List<Collision2D> collist;

    // Start is called before the first frame update
    void Start()
    {
      //  collist = new List<Collision2D>();
        stop = 1;
        noStop = 0;
        waitTime = 0.5f;
        //currentScreenWidth = Screen.width;
        //currentScreenHeight = Screen.height;
    }
    private void FixedUpdate() {

        // block wenn alert screen pops up
        if (globals.AlertScreen == true) {
            return;
        }

        if (waitTime > 0 && stop != 1) {
            waitTime = waitTime - Time.fixedDeltaTime;
        }
        
        ////  transform.localScale = grid.transform.localScale;
        //  Debug.Log("size debug");
        //  if (currentScreenHeight != Screen.height) {
        //      float newScreenHeight = Screen.height;
        //      Debug.Log("last x size");
        //      Debug.Log(currentScreenHeight);
        //      Debug.Log("current x size");
        //      Debug.Log(newScreenHeight);

        //      heightChange((newScreenHeight / currentScreenHeight));
        //      currentScreenHeight = newScreenHeight;

        //  }

        //  if (currentScreenWidth != Screen.width) {
        //      float newScreenWidth = Screen.width;

        //      Debug.Log("last y size");
        //      Debug.Log(currentScreenWidth);
        //      Debug.Log("current y size");
        //      Debug.Log(newScreenWidth);

        //      widthChange((newScreenWidth / currentScreenWidth));
        //      currentScreenWidth = newScreenWidth;

        //  }
        //  Debug.Log("position");


        if (stop == 0 && waitTime <0) {
            Vector3 v3 = transform.position;

            v3.y = v3.y - speed;

            transform.position = v3;

        }

        if ((transform.localPosition.y + (globals.mainPanel.rect.height / 2)) <= 0) {


            Destroy(gameObject);
           // Debug.Log("power up zerstört ( runtergefallen)");
        }

        //private void widthChange(float sizeRelation) {
        //    Vector3 v3 = transform.position;

        //    v3.x = v3.x * sizeRelation;

        //    transform.position = v3;

        //}
        //private void heightChange(float sizeRelation) {

        //    Vector3 v3 = transform.position;

        //    v3.y = v3.y * sizeRelation;

        //    transform.position = v3;
        //
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        // block wenn alert screen pops up
        if (globals.AlertScreen == true) {
            return;
        }

        //  collist.Add(collision);
       
        if (globals.ballList.Contains(collision.gameObject)) {
            // exclude ball
        }
        else if(globals.powerupGameObjectList.Contains(collision.gameObject)) {
            // exclude powerups
        }
        else {
            if (noStop == 0) {

                stop = 1;
               
                //Debug.Log("stop = 1");


                //Debug.Log(hit.normal.y);
            }  
        }

        if(collision.gameObject == globals.padle) {


            int size = globals.powerupList.Count;
            int i = 0;
            while (i < size) {
                string s2 = gameObject.GetComponent<SpriteRenderer>().sprite.ToString();
                s2 = s2.Replace(" (UnityEngine.Sprite)", ""); // scheinbar schreibt es das jedesmal in den string mit rein beim converten
                string s = globals.powerupList[i];

                if (s == s2) {

                    // die classe muss instasziert werden wegen, weil monobehavor benötigt ist
                    power_up_effects p = new power_up_effects();

                    if (i == 0) {
                        p.extraBalls();
                        Debug.Log("power up extra balls");
                    }
                    else if (i == 1) {
                        p.ballFast();
                        Debug.Log("power up fast");
                    }
                    else if(i == 2) {
                        p.padleGrow();
                        Debug.Log("power up grow");
                    }
                    else if(i == 3) {
                        p.ballPierce();
                        Debug.Log("power up pierce");
                    }
                    else if(i == 4) {
                        p.padleShrink();
                        Debug.Log("power up shrink");
                    }
                    else if(i == 5) {
                        p.ballSlow();
                        Debug.Log("power up slow");
                    }

                }

                i = i + 1;
            }
            

            //foreach (string s in globals.powerupList) {
            //    //  Debug.Log(s.Length);
            //    string s2 = gameObject.GetComponent<SpriteRenderer>().sprite.ToString();
            //    s2 = s2.Replace(" (UnityEngine.Sprite)", ""); // scheinbar schreibt es das jedesmal in den string mit rein beim converten

            //    if (s == s2) {

            //    }
            //}

            Destroy(gameObject);
           // Debug.Log("power up zerstört");
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision) {

        // block wenn alert screen pops up
        if (globals.AlertScreen == true) {
            return;
        }

        if (globals.ballList.Contains(collision.gameObject)) {
            // exclude ball
        }
        else if (globals.powerupGameObjectList.Contains(collision.gameObject)) {
            // exclude powerups
        }
        else {
            stop = 1;
            waitTime = 0.5f;
        }
        
        
    }

    private void OnCollisionExit2D(Collision2D collision) {

        // block wenn alert screen pops up
        if (globals.AlertScreen == true) {
            return;
        }

        stop = 0;
        noStop = 1;
     //   Debug.Log("stop = 0");
    }

}
