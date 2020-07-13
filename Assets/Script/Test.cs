using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Text testText;
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 a = Input.acceleration;
        testText.text = a.ToString();
    }
}
