using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTrans : MonoBehaviour
{
    public static ScreenTrans instance;

    private void Start()
    {
        instance = this;
    }

    public void Play(System.Action callback)
    {
        StartCoroutine(PlayCoroutine(callback));
    }

    IEnumerator PlayCoroutine(System.Action callback)
    {
        this.GetComponent<Animator>().SetTrigger("01");
        yield return new WaitForSeconds(0.5f);
        callback();
    }
}
