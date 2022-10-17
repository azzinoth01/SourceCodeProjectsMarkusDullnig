using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class power_up_effects :MonoBehaviour
{

    
    public void ballSlow() {
        foreach(GameObject g in globals.ballList) {

            float speed = g.GetComponent<ballmove>().speed;

            speed = speed / 2;

            // minimal geschwindigkeit
            if(speed < 0.065f) {
                speed = 0.065f;
            }
            g.GetComponent<ballmove>().speed = speed;
        }
    }
    public void ballFast() {
        foreach (GameObject g in globals.ballList) {

            float speed = g.GetComponent<ballmove>().speed;

            speed = speed * 2;

            // maximal geschwindigkeit
            if (speed > 0.26f) {
                speed = 0.26f;
            }
            g.GetComponent<ballmove>().speed = speed;
        }
    }
    public void padleGrow() {
        Rect  r = globals.padle.GetComponent<RectTransform>().rect;

        r.width = r.width * 2;

        if(r.width > 20) {
            r.width = 20;
        }

       // globals.padle.GetComponent<RectTransform>().rect.Set(r.x,r.y,r.width,r.height);

        globals.padle.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, r.width);

    }

    public void padleShrink() {
        Rect r = globals.padle.GetComponent<RectTransform>().rect;

        r.width = r.width / 2;

        if (r.width < 2.5f) {
            r.width = 2.5f;
        }

    //    globals.padle.GetComponent<RectTransform>().rect.Set(r.x, r.y, r.width, r.height);
        globals.padle.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, r.width);
    }

    public void ballPierce() {
        foreach (GameObject g in globals.ballList) {

            g.GetComponent<ballmove>().pierce = 1;

        }
    }
    public void extraBalls() {
        foreach (GameObject g in globals.ballList) {

            GameObject ballPrefab =  g.GetComponent<ballmove>().ballPrefab;

            GameObject extra1 = Instantiate(ballPrefab, g.transform.parent);
            

            float angel = g.GetComponent<ballmove>().angel;
            int xDirection = g.GetComponent<ballmove>().xDirection;
            int yDirection = g.GetComponent<ballmove>().yDirection;
            int originBallPos = 0;

            if(yDirection ==1) {
                if(angel < 90) {
                    if ((angel - 45) > 1) {
                        angel = angel - 45;
                        originBallPos = 0;
                        xDirection = 1;
                    }
                    else {
                        angel = angel + 45;
                        originBallPos = 1;
                        if(angel > 90) {
                            xDirection = -1;
                        }
                        else {
                            xDirection = 1;
                        }
                        
                    }
                }
                else {
                    if ((angel + 45) < 179) {
                        angel = angel + 45;
                        originBallPos = 0;
                        xDirection = -1;
                    }
                    else {
                        angel = angel - 45;
                        originBallPos = -1;
                        if (angel > 90) {
                            xDirection = -1;
                        }
                        else {
                            xDirection = 1;
                        }
                    }
                }
               
            }
            else {
                if(angel < 90) {
                    if ((angel - 45) > 1) {
                        angel = angel - 45;
                        originBallPos = 0;
                        xDirection = -1;
                    }
                    else {
                        angel = angel + 45;
                        originBallPos = -1;
                        if (angel > 90) {
                            xDirection = 1;
                        }
                        else {
                            xDirection = -1;
                        }
                    }
                }
                else {
                    if ((angel + 45) < 179) {
                        angel = angel + 45;
                        originBallPos = 0;
                        xDirection = 1;
                    }
                    else {
                        angel = angel - 45;
                        originBallPos = -1;
                        if (angel > 90) {
                            xDirection = 1;
                        }
                        else {
                            xDirection = -1;
                        }
                    }
                }
               
            }

           

            extra1.GetComponent<ballmove>().extraBall = 1;
            extra1.GetComponent<ballmove>().grid = globals.mainGrid;
            extra1.GetComponent<ballmove>().panel = globals.mainPanel;
            extra1.GetComponent<ballmove>().padle = g.GetComponent<ballmove>().padle;
            extra1.GetComponent<ballmove>().ballPrefab = g.GetComponent<ballmove>().ballPrefab;
            extra1.GetComponent<RectTransform>().position = g.GetComponent<RectTransform>().position;


            extra1.GetComponent<ballmove>().xDirection = xDirection;
            extra1.GetComponent<ballmove>().yDirection = yDirection;
            extra1.GetComponent<ballmove>().angel = angel;
            extra1.GetComponent<ballmove>().hit_audio = g.GetComponent<ballmove>().hit_audio;
            extra1.GetComponent<ballmove>().animationPrefab = g.GetComponent<ballmove>().animationPrefab;


            float angel2;
            int xDirection2;
            int yDirection2 = g.GetComponent<ballmove>().yDirection;

            if (angel > 90) {
                angel2 = angel - 90;
                if (originBallPos == 0) {
                    xDirection2 = xDirection * -1;
                }
                else if(originBallPos == -1) {
                    xDirection2 = 1;
                }
                else {
                    xDirection2 = -1;
                }
               
            }
            else {
                angel2 = angel + 90;
                if (originBallPos == 0) {
                    xDirection2 = xDirection * -1;
                }     
                else if (originBallPos == -1) {
                    xDirection2 = -1;
                }
                else {
                    xDirection2 = 1;
                }
            }
         
            
            

            GameObject extra2 = Instantiate(ballPrefab, g.transform.parent);

            angel2 = angel2 + 45;

            extra2.GetComponent<ballmove>().extraBall = 1;
            extra2.GetComponent<ballmove>().grid = globals.mainGrid;
            extra2.GetComponent<ballmove>().panel = globals.mainPanel;
            extra2.GetComponent<ballmove>().padle = g.GetComponent<ballmove>().padle;
            extra2.GetComponent<ballmove>().ballPrefab = g.GetComponent<ballmove>().ballPrefab;
            extra2.GetComponent<RectTransform>().position = g.GetComponent<RectTransform>().position;

            extra2.GetComponent<ballmove>().xDirection = xDirection2;
            extra2.GetComponent<ballmove>().yDirection = yDirection2;
            extra2.GetComponent<ballmove>().angel = angel2;
            extra2.GetComponent<ballmove>().hit_audio = g.GetComponent<ballmove>().hit_audio;
            extra2.GetComponent<ballmove>().animationPrefab = g.GetComponent<ballmove>().animationPrefab;

        }
    }

}
