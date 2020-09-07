using Spine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TakePhoto : MonoBehaviour
{
    public Transform AlbumContext;
    public void TakePhotoScreen()
    {
        int index = ScreenShotHandler.instance.LastIndex();

        if (AlbumContext.childCount < index)
        {
            OverrideCanvas.instance.RedAlram("앨범 허용량 초과");
            return;
        }

        ScreenShotHandler.TakeScreenshot_Static(540, 1080, index);

        ScreenTrans.instance.ScreenTrans05(() => {
            ScreenShotHandler.instance.SystemIOFileLoad(index, () => {
                OverrideCanvas.instance.PolaroidPhoto(ScreenShotHandler.instance.loadSprite[index], 8, 17);
            });
        });
    }
}
