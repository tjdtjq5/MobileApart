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

        Body("body");
        eye("eye/eye_01");
        face("face/face_01");
        haF("haF/hair_f_01");
        haB("haB/hair_b_01");
        top("top/clo_top_01");

        Debug.Log(SetColor("top", Color.black, 2));
    }

    public bool SetColor(string slotName , Color color, int slotNum = 1)
    {
        bool flag = false; 
        foreach (Spine.Slot slot in skeletonAnimation.skeleton.Slots)
        {
            if (slot.Attachment != null)
            {
                if (slotNum == 2)
                {
                    if (slot.Data.Name.Contains(slotName) && slot.Data.Name.Contains("color_02"))
                    {
                        int slotIndex = slot.Data.Index;
                        Attachment attachment = skeletonAnimation.Skeleton.GetAttachment(slotIndex, slot.Attachment.Name);

                        var slot2 = skeletonAnimation.Skeleton.Slots.Items[slotIndex];
                        slot.Attachment = attachment;

                        ChangeAttachmentColor(attachment, color);
                        flag = true;
                    }
                }
                else
                {
                    if (slot.Data.Name.Contains(slotName) && slot.Data.Name.Contains("color_01"))
                    {
                        int slotIndex = slot.Data.Index;
                        Attachment attachment = skeletonAnimation.Skeleton.GetAttachment(slotIndex, slot.Attachment.Name);

                        var slot2 = skeletonAnimation.Skeleton.Slots.Items[slotIndex];
                        slot.Attachment = attachment;

                        ChangeAttachmentColor(attachment, color);
                        flag = true;
                    }
                }
           
            }
        }
        return flag;
    }

    //public Color GetColor(string slotName, int slotNum = 1)
    //{
    //    foreach (Spine.Slot slot in skeletonAnimation.skeleton.Slots)
    //    {
    //        if (slot.Attachment != null)
    //        {
    //            if (slotNum == 2)
    //            {
    //                if (slot.Data.Name.Contains(slotName) && slot.Data.Name.Contains("color_02"))
    //                {
    //                    int slotIndex = slot.Data.Index;
    //                    Attachment attachment = skeletonAnimation.Skeleton.GetAttachment(slotIndex, slot.Attachment.Name);

    //                    var slot2 = skeletonAnimation.Skeleton.Slots.Items[slotIndex];
    //                    slot.Attachment = attachment;

    //                    ChangeAttachmentColor(attachment, color);
    //                    flag = true;
    //                }
    //            }
    //            else
    //            {
    //                if (slot.Data.Name.Contains(slotName) && slot.Data.Name.Contains("color_01"))
    //                {
    //                    int slotIndex = slot.Data.Index;
    //                    Attachment attachment = skeletonAnimation.Skeleton.GetAttachment(slotIndex, slot.Attachment.Name);

    //                    var slot2 = skeletonAnimation.Skeleton.Slots.Items[slotIndex];
    //                    slot.Attachment = attachment;

    //                    ChangeAttachmentColor(attachment, color);
    //                    flag = true;
    //                }
    //            }

    //        }
    //    }
    //}

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

    public void SkinChange(SkinKind skinKind, string skinName )
    {
        switch (skinKind)
        {
            case SkinKind.acc:
                acc(skinName);
                break;
            case SkinKind.top:
                top(skinName);
                break;
            case SkinKind.pan:
                pan(skinName);
                break;
            case SkinKind.eye:
                eye(skinName);
                break;
            case SkinKind.face:
                face(skinName);
                break;
            case SkinKind.haF:
                haF(skinName);
                break;
            case SkinKind.haB:
                haB(skinName);
                break;
            case SkinKind.outt:
                outt(skinName);
                break;
            case SkinKind.sho:
                sho(skinName);
                break;
            case SkinKind.cap:
                cap(skinName);
                break;
            case SkinKind.set:
                set(skinName);
                break;
            default:
                break;
        }



    }
 
    void Body(string skinName)
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
    void acc(string skinName)
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

    void top(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("top"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
     
    }

    void pan(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("pan"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    void eye(string skinName)
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

    void face(string skinName)
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
    void haF(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("haF"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
      
    }
    void haB(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("haB"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
     
    }
    void outt(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("outt"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
      
    }
    void sho(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("sho"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    void cap(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("cap"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    void set(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("set"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    void SetEquip(List<string> SkinList)
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
