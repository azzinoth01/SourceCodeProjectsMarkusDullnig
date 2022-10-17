using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class take_screen_shot : MonoBehaviour
{
    public Canvas canvasToSreenShot;
    public RectTransform panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    //    Debug.Log("Screen width");
    //    Debug.Log(Screen.width);
    //    Debug.Log("Screen height");
    //    Debug.Log(Screen.height);

    //    Debug.Log("panel width");
    //    Debug.Log(panel.rect.width);
    //    Debug.Log("panel height");
    //    Debug.Log(panel.rect.height);
    //    Debug.Log("scaler");
    //    Debug.Log(canvasToSreenShot.scaleFactor);
        if(Input.anyKey == true) {
            Debug.Log("anyKey");
            //Subscribe
            CanvasScreenShot.OnPictureTaken += receivePNGScreenShot;
            CanvasScreenShot screenShot = gameObject.GetComponent<CanvasScreenShot>();

            //take ScreenShot(Image and Text)
            //screenShot.takeScreenShot(canvasToSreenShot, SCREENSHOT_TYPE.IMAGE_AND_TEXT, false);
            //take ScreenShot(Image only)
            screenShot.takeScreenShot(canvasToSreenShot, panel, SCREENSHOT_TYPE.IMAGE_AND_TEXT, false);
            //take ScreenShot(Text only)
            // screenShot.takeScreenShot(canvasToSreenShot, SCREENSHOT_TYPE.TEXT_ONLY, false);

        }
    }

    void receivePNGScreenShot(byte[] pngArray) {
        Debug.Log("Picture taken");

        //Do Something With the Image (Save)
        string path = Application.persistentDataPath + "/CanvasScreenShot.png";
        System.IO.File.WriteAllBytes(path, pngArray);
        Debug.Log(path);
    }

}
