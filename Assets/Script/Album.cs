using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Album : MonoBehaviour
{
    public GameObject pannel;

    public void OpenAlbum()
    {
        pannel.SetActive(true);
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
        for (int i = 0; i < pannel.transform.childCount; i++)
        {
            pannel.transform.GetChild(i).GetComponent<Image>().sprite = null;
        }
    }
}
