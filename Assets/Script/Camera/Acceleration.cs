using DG.Tweening;
using UnityEngine;

public class Acceleration : MonoBehaviour
{
    public float multyple;

    bool stopFlag = false;
    void Update()
    {
        if (!stopFlag)
        {
            Vector3 a = Input.acceleration;
            if (a.x > 0)
            {
                //this.transform.DORotate(new Vector3(Mathf.Pow(a.y * multyple, 2), Mathf.Pow(a.x * multyple, 2), 0), .6f);
            }
        }
    }

    public void StopRotation()
    {
        stopFlag = true;
        this.transform.DORotate(new Vector3(0, 0, 0), 0.3f);
    }

    public void DonStopRotation()
    {
        stopFlag = true;
    }


}
