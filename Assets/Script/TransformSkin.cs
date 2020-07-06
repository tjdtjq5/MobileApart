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
        UserEqipInfoSetting();
        UserEqipInfoSetting();
    }

    // 유저정보에 저장된 eqip 정보 세팅 
    public void UserEqipInfoSetting()
    {
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.acc).skinName != "")
        {
            acc(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.acc).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.acc).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.acc).color_01 , 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.acc).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.acc).color_02 , 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.body).skinName != "")
        {
            body(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.body).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.body).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.body).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.body).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.body).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.cap).skinName != "")
        {
            cap(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.cap).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.cap).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.cap).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.cap).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.cap).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.eye).skinName != "")
        {
            eye(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.eye).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.eye).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.eye).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.eye).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.eye).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.face).skinName != "")
        {
            face(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.face).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.face).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.face).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.face).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.face).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB).skinName != "")
        {
            haB(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF).skinName != "")
        {
            haF(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.outt).skinName != "")
        {
            outt(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.outt).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.outt).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.outt).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.outt).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.outt).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.pan).skinName != "")
        {
            pan(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.pan).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.pan).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.pan).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.pan).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.pan).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.set).skinName != "")
        {
            set(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.set).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.set).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.set).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.set).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.set).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.sho).skinName != "")
        {
            sho(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.sho).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.sho).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.sho).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.sho).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.sho).color_02, 2);
        }
        if (GameManager.instance.userInfoManager.GetUserEqip(SkinKind.top).skinName != "")
        {
            top(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.top).skinName);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.top).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.top).color_01, 1);
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.top).skinName, GameManager.instance.userInfoManager.GetUserEqip(SkinKind.top).color_02, 2);
        }
    }

    public bool SetColor(string slotName , Color color, int slotNum = 1)
    {
        if (color == null)
        {
            return false;
        }
        if (color == Color.clear)
        {
            float randR = (float)Random.RandomRange(0, 255) / 255;
            float randG = (float)Random.RandomRange(0, 255) / 255;
            float randB = (float)Random.RandomRange(0, 255) / 255;
            color = new Color(randR, randG, randB, 1);
        }

        bool flag = false;

        if (slotName.Contains("haB") && slotNum == 1)
        {
            color = GetColor(GameManager.instance.userInfoManager.haF.skinName , 1);
        }
        if (slotName.Contains("haB") && slotNum == 2)
        {
            color = GetColor(GameManager.instance.userInfoManager.haF.skinName, 2);
        }

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

        if (slotName.Contains("haF") && slotNum == 1)
        {
            SetColor(GameManager.instance.userInfoManager.haB.skinName, color);
        }
        if (slotName.Contains("haF") && slotNum == 2)
        {
            SetColor(GameManager.instance.userInfoManager.haB.skinName, color);
        }

        return flag;
    }

    public Color GetColor(string slotName, int slotNum = 1)
    {
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

                        RegionAttachment regionAttachment = attachment as RegionAttachment;
                        if (regionAttachment != null)
                        {
                            return regionAttachment.GetColor();
                        }
                        MeshAttachment meshAttachment = attachment as MeshAttachment;
                        if (meshAttachment != null)
                        {
                            return meshAttachment.GetColor();
                        }
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

                        RegionAttachment regionAttachment = attachment as RegionAttachment;
                        if (regionAttachment != null)
                        {
                            return regionAttachment.GetColor();
                        }
                        MeshAttachment meshAttachment = attachment as MeshAttachment;
                        if (meshAttachment != null)
                        {
                            return meshAttachment.GetColor();
                        }
                    }
                }

            }
        }
        return Color.clear;
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
 
    void body(string skinName)
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
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();
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
