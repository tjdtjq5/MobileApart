﻿using BackEnd.Tcp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Album : MonoBehaviour
{
    [Header("이미지 패널")]
    public Transform ImgPannel;
    IEnumerator OpenCoroutine;
  
    public void AlbumOpen()
    {
        int index = ScreenShotHandler.instance.LastIndex();

        for (int i = index; i < ImgPannel.childCount; i++)
        {
            ImgPannel.GetChild(i).gameObject.SetActive(false);
        }

        OpenCoroutine = LoadSpriteCoroutine();
        StartCoroutine(OpenCoroutine);

        this.transform.Find("휴지통").GetComponent<Button>().onClick.RemoveAllListeners();
        this.transform.Find("휴지통").GetComponent<Button>().onClick.AddListener(() => { Treash(); });
    }
    bool loadFlag = false;
    void LoadSprite(int iCount)
    {
        loadFlag = true;
        ScreenShotHandler.instance.SystemIOFileLoad(iCount, () => {
            ImgPannel.GetChild(iCount).gameObject.SetActive(true);
            ImgPannel.GetChild(iCount).GetChild(0).GetComponent<Image>().sprite = ScreenShotHandler.instance.loadSprite[iCount];
            ImgPannel.GetChild(iCount).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            ImgPannel.GetChild(iCount).GetChild(1).gameObject.SetActive(false);
            loadFlag = false;
        });
    }
    IEnumerator LoadSpriteCoroutine()
    {
        int index = ScreenShotHandler.instance.LastIndex();
        int iCount = 0;
        while(iCount < index)
        {
            if (!loadFlag)
            {
                LoadSprite(iCount);
                iCount++;
            }
            yield return null;
        }
    }

    public void AlbumClose()
    {
        StopCoroutine(OpenCoroutine);
        ScreenShotHandler.instance.AllStopLoad();
    }

    public void Treash()
    {
        for (int i = 0; i < ImgPannel.childCount; i++)
        {
            if (ImgPannel.GetChild(i).gameObject.activeSelf)
            {
                ImgPannel.GetChild(i).GetChild(1).gameObject.SetActive(true);
                ImgPannel.GetChild(i).GetChild(1).GetChild(0).gameObject.SetActive(false);

                int index = i;
                ImgPannel.GetChild(index).GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
                    if (ImgPannel.GetChild(index).GetChild(1).GetChild(0).gameObject.activeSelf)
                    {
                        ImgPannel.GetChild(index).GetChild(1).GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        ImgPannel.GetChild(index).GetChild(1).GetChild(0).gameObject.SetActive(true);
                    }
                });
            }
        }


        this.transform.Find("휴지통").GetComponent<Button>().onClick.RemoveAllListeners();
        this.transform.Find("휴지통").GetComponent<Button>().onClick.AddListener(() => { TreashCheck(); });
    }

    void TreashCheck()
    {
        List<int> removeList = new List<int>();

        for (int i = 0; i < ImgPannel.childCount; i++)
        {
            if (ImgPannel.GetChild(i).gameObject.activeSelf)
            {
                if (ImgPannel.GetChild(i).GetChild(1).GetChild(0).gameObject.activeSelf)
                {
                    removeList.Add(i);
                }
            }
        }

        OverrideCanvas.instance.Caution("정말로 삭제 하시겠습니까?",()=> { Remove(removeList); });
    }

    void Remove(List<int> removeList)
    {
        StartCoroutine(RemoveCoroutine(removeList));
    }

    IEnumerator RemoveCoroutine(List<int> removeList)
    {
        removeList.Sort();
        removeList.Reverse();

        OverrideCanvas.instance.Wating("사진을 삭제하는 중 ...", true);
        for (int i = 0; i < removeList.Count; i++)
        {
            ScreenShotHandler.instance.Remove(removeList[i]);
            yield return new WaitForSeconds(0.1f);
        }
        OverrideCanvas.instance.Wating("사진을 삭제하는 중 ...", false);
        AlbumOpen();
    }
}
