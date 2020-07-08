using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Album : MonoBehaviour
{
    public GameObject pannel;
    public GameObject exitBtn;

    public void OpenAlbum()
    {
        pannel.SetActive(true);
        exitBtn.SetActive(true);

        for (int i = 0; i < pannel.transform.childCount; i++)
        {
            Sprite tempSprite = ScreenShotHandler.instance.SystemIOFileLoad(i);
            if (tempSprite == null) return;
            pannel.transform.GetChild(i).GetComponent<Image>().sprite = tempSprite;
        }
    }

    public void CloseAlbum()
    {
        pannel.SetActive(false);
        exitBtn.SetActive(false);

        for (int i = 0; i < pannel.transform.childCount; i++)
        {
            pannel.transform.GetChild(i).GetComponent<Image>().sprite = null;
        }
    }
}
