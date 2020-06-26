using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpineSkinInfoManager : MonoBehaviour
{
    public SpineSkinInfo[] SpineSkinInfoList;

    public SpineSkinInfo GetSpineSkinInfo(string skinKind)
    {
        for (int i = 0; i < SpineSkinInfoList.Length; i++)
        {
            if (SpineSkinInfoList[i].skinKind == skinKind)
            {
                return SpineSkinInfoList[i];
            }
        }
        return new SpineSkinInfo();
    }

    [Serializable]
    public struct SpineSkinInfo
    {
        public string skinKind;
        public string[] skinName;
    }
}
