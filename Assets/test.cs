using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Spine;

public class test : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    private void Start()
    {
      //  string[] skinNameList = { , "face/face_01" };
        List<string> skinNameList = new List<string>();
        skinNameList.Add("hair_b/hair_01");
        skinNameList.Add("face/face_01");
    }

    

}


