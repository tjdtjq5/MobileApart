using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TakePhoto : MonoBehaviour
{
    public void TakePhotoScreen()
    {
        int index = ScreenShotHandler.instance.LastIndex();
        ScreenShotHandler.TakeScreenshot_Static(Screen.width, Screen.height, index);

        ScreenTrans.instance.ScreenTrans05(() => {
            OverrideCanvas.instance.PolaroidPhoto(ScreenShotHandler.instance.SystemIOFileLoad(index), 8, 17);
        });
    }
}
