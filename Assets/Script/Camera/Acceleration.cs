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
            float x = 0;
            float y = 0;
            if (a.y > 0)
            {
                x = Mathf.Pow(Mathf.Abs(a.y) * multyple , 2);
            }
            else
            {
                x = -Mathf.Pow(Mathf.Abs(a.y) * multyple , 2);
            }
            if (a.x > 0)
            {
                y = Mathf.Pow(Mathf.Abs(a.x) * multyple * 1.8f, 2);
            }
            else
            {
                y = -Mathf.Pow(Mathf.Abs(a.x) * multyple * 1.8f, 2);
            }

            this.transform.DORotate(new Vector3(x, y, 0), .6f);
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
