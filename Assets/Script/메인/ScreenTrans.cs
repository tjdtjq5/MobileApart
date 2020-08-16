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
        yield return new WaitForSeconds(1);
        callback();
    }

    public void ScreenTrans05(System.Action callback)
    {
        StartCoroutine(ScreenTrans05Coroutine(callback));
    }

    IEnumerator ScreenTrans05Coroutine(System.Action callback)
    {
        this.GetComponent<Animator>().SetTrigger("05");
        yield return new WaitForSeconds(0.1f);
        callback();
    }
}
