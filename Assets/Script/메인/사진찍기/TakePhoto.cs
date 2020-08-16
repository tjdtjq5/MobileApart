using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TakePhoto : MonoBehaviour
{
    public void TakePhotoScreen(int index)
    {
        ScreenTrans.instance.ScreenTrans05(() => {
           // ScreenShotHandler.TakeScreenshot_Static(Screen.width, Screen.height, index);
            ScreenShotHandler.instance.Remove(2);
        });
    }
}
