using JetBrains.Annotations;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformSkin : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    List<string> skinList = new List<string>();
    //public Color color;
    public Slider R;
    public Slider G;
    public Slider B;

    public void SetColor()
    {
        foreach (Spine.Slot slot in skeletonAnimation.skeleton.Slots)
        {
            if (slot.Attachment != null)
            {
                if (slot.Attachment.Name.Contains("hair"))
                {
                    Color color = new Color((R.value), (G.value), (B.value), 1);
                    slot.SetColor(color);
                }
            }
        }
    }

    private void Start()
    {
        skeletonAnimation = transform.GetComponent<SkeletonAnimation>();
        Hair_f("hair_f/hair_01");
        Hair_b("hair_b/hair_01");
        Face("face/face_01");
        Eye("eye/eye_01");
        Clo_Under("clo_under/clo_under_01");
        Clo_Top("clo_top/clo_top01");
    }

    public void Hair_f(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("hair_f"))
            {
                skinList.RemoveAt(i);
            }
        }
        Debug.Log(skinList.Count);
        skinList.Add(skinName);
        SetEquip(skinList);

       
    }

    public void Hair_b(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("hair_b"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    public void Face(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("face"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    public void Eye(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("eye"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    public void Clo_Under(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("clo_under"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }
    public void Clo_Top(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("clo_top"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }
    public void Outer(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("outer"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }
    public void Acc(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("acc"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }
    public void Race(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("race"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    public void SetEquip(List<string> SkinList)
    {
        Skin combined = new Skin("combined");

        foreach (var skinName in SkinList)
        {
            Skin skin = skeletonAnimation.skeleton.Data.FindSkin(skinName);

            if (skin != null)
            {
                combined.AddFromSkin(skin);
            }
        }

        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.skeleton.SetSkin(combined);
    }
}
