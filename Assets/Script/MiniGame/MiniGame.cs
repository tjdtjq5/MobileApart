using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MiniGame : MonoBehaviour
{
    public GameObject mainScreen;
    public Image[] mainImg;
    public Image[] miniGameImg;

    float fadeSpeed = 0.3f;
    public void MiniGameIn()
    {
        StartCoroutine(MainGameInCoroutine());
    }

    public void MiniGameOut()
    {
        StartCoroutine(MainGameOutCoroutine());
    }

    IEnumerator MainGameInCoroutine()
    {
        mainImg[0].DOFade(0, fadeSpeed).OnComplete(() => { mainScreen.SetActive(false); });
        for (int i = 1; i < mainImg.Length; i++)
        {
            mainImg[i].DOFade(0, fadeSpeed);
        }
        yield return new WaitForSeconds(fadeSpeed);

        for (int i = 0; i < miniGameImg.Length; i++)
        {
            miniGameImg[i].gameObject.SetActive(true);
            miniGameImg[i].DOFade(1, fadeSpeed);
        }
    }

    IEnumerator MainGameOutCoroutine()
    {
        miniGameImg[0].DOFade(0, fadeSpeed).OnComplete(() => { mainScreen.SetActive(true); });
        for (int i = 1; i < miniGameImg.Length; i++)
        {
            miniGameImg[i].DOFade(0, fadeSpeed);
        }
        yield return new WaitForSeconds(fadeSpeed);

        for (int i = 0; i < miniGameImg.Length; i++)
        {
            miniGameImg[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < mainImg.Length; i++)
        {
            mainImg[i].gameObject.SetActive(true);
            mainImg[i].DOFade(1, fadeSpeed);
        }
    }
}
