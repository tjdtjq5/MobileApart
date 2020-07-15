using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
        StartCoroutine(DelayedRendering());
    }

    public IEnumerator DelayedRendering()
    {
        while (true)
        {
            cam.Render();
            yield return new WaitForSeconds(0.0466667f);
        }
    }

}
