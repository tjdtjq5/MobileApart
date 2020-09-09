using BackEnd;
using LitJson;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coopon : MonoBehaviour
{
    [ContextMenu("테스트")]
    public void Test()
    {
        UseCoopon("4rscvqy2kxfghm3wyx");
    }

    public void UseCoopon(string serialNumber)
    {
        Backend.Coupon.UseCoupon(serialNumber, (callback) =>
        {
            if (callback.IsSuccess())
            {
                JsonData jsonData = callback.GetReturnValuetoJSON();
                Debug.Log(jsonData["uuid"].ToString());
                Debug.Log(jsonData["items"]["itemName"].ToString());
                Debug.Log(jsonData["itemsCount"].ToString());
            }
            else
            {
                Debug.Log("쿠폰 입력 실패 (에러코드 : " + callback.GetErrorCode() +  ")");
            }
           
        });
    }
}
