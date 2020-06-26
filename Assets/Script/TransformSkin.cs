using JetBrains.Annotations;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class TransformSkin : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    List<string> skinList = new List<string>();
 

    private void Start()
    {
        skeletonAnimation = transform.GetComponent<SkeletonAnimation>();
        Hair_f("hair_f/hair_f_01");
        Hair_b("hair_b/hair_b_01");
        Face("face/face_01");
        Eye("eye/eye_01");
        Clo_Under("clo_under/clo_under_01");
        Clo_Top("clo_top/clo_top01");
        Body("body");

       
    }

    public void SetColor(string slotName , Color color)
    {
       
        foreach (Spine.Slot slot in skeletonAnimation.skeleton.Slots)
        {
            if (slot.Attachment != null)
            {
                if (slot.Data.Name.Contains(slotName) && slot.Attachment.Name.Contains("color_01"))
                {
                    int slotIndex = slot.Data.Index;
                    Attachment attachment = skeletonAnimation.Skeleton.GetAttachment(slotIndex, slot.Attachment.Name);
                    
                    var slot2 = skeletonAnimation.Skeleton.Slots.Items[slotIndex];
                    slot.Attachment = attachment;

                    ChangeAttachmentColor(attachment, color);

                }
            }
        }
       


      
    }

    public void RandomSetColor(string slotName)
    {
        float randR = (float)Random.RandomRange(0, 255) / 255;
        float randB = (float)Random.RandomRange(0, 255) / 255;
        float randG = (float)Random.RandomRange(0, 255) / 255;

        foreach (Spine.Slot slot in skeletonAnimation.skeleton.Slots)
        {
            if (slot.Attachment != null)
            {
                if (slot.Attachment.Name.Contains(slotName) && slot.Attachment.Name.Contains("color_01"))
                {

                    Color color = new Color((randR), (randG), (randB), 1);

                    int slotIndex = slot.Data.Index;
                    Attachment attachment = skeletonAnimation.Skeleton.GetAttachment(slotIndex, slot.Attachment.Name);

                    var slot2 = skeletonAnimation.Skeleton.Slots.Items[slotIndex];
                    slot.Attachment = attachment;

                    ChangeAttachmentColor(attachment, color);

                  
                }
            }
        }
    }

    void ChangeAttachmentColor(Attachment attachment, Color color)
    {
        RegionAttachment regionAttachment = attachment as RegionAttachment;
        if (regionAttachment != null)
        {
            regionAttachment.SetColor(color);
        }
        MeshAttachment meshAttachment = attachment as MeshAttachment;
        if (meshAttachment != null)
        {
            meshAttachment.SetColor(color);
        }
    }

    
 
    public void Body(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("body"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);

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

    // 이거를 Skin.cs 스크립트에 넣어줘야한다. 
    //public void AddFromSkin(Skin other)
    //{
    //    foreach (var a in other.attachments)
    //    {
    //        attachments[a.Key] = a.Value;
    //    }
    //}
}
