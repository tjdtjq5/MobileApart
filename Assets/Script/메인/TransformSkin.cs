using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TransformSkin : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    List<string> skinList = new List<string>();

    [ContextMenu("테스트")]
    public void Test()
    {
        int skinKindLength = System.Enum.GetNames(typeof(SkinKind)).Length;
        for (int i = 0; i < skinKindLength; i++)
        {
            string skinName = GameManager.instance.userInfoManager.GetUserEqip((SkinKind)i).skinName;
            if (skinName.Length != 0)
            {
                Debug.Log((SkinKind)i + " : " + GameManager.instance.userInfoManager.GetUserEqip((SkinKind)i).color_01);
                SetColor(GameManager.instance.userInfoManager.GetUserEqip((SkinKind)i).skinName, GameManager.instance.userInfoManager.GetUserEqip((SkinKind)i).color_01);
            }
        }
    }

    private void Start()
    {
        skeletonAnimation = transform.GetComponent<SkeletonAnimation>();
        UserEqipInfoSetting();
    }

    IEnumerator tempAnimationCoroutine;
    public void Animation(string ani, float time, bool loop = false, string skinName = "")
    {
        if (tempAnimationCoroutine != null)
        {
            StopCoroutine(tempAnimationCoroutine);
        }
        tempAnimationCoroutine = AnimationCoroutine(ani, time, loop, skinName);
        StartCoroutine(tempAnimationCoroutine);
    }

    IEnumerator AnimationCoroutine(string ani, float time, bool loop = false, string skinName = "")
    {
        if (skinName != "")
        {
            skeletonAnimation.skeleton.Skin = null;
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            skeletonAnimation.LateUpdate();

            List<string> tempSkinList = skinList;
            tempSkinList.Add(skinName);
            SetEquip(tempSkinList);
        }
        else
        {
            skeletonAnimation.skeleton.Skin = null;
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            skeletonAnimation.LateUpdate();
            SetEquip(skinList);
        }
        skeletonAnimation.AnimationState.SetAnimation(0, ani, loop);
        yield return new WaitForSeconds(time);
        skeletonAnimation.AnimationState.SetAnimation(0, GameManager.instance.userInfoManager.currentAnimation, true);

        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();
        SetEquip(skinList);
    }

    // 유저정보에 저장된 eqip 정보 세팅 
    public void UserEqipInfoSetting()
    {
        int skinKindLength = System.Enum.GetNames(typeof(SkinKind)).Length;
        for (int i = 0; i < skinKindLength; i++)
        {
            string skinName = GameManager.instance.userInfoManager.GetUserEqip((SkinKind)i).skinName;
            if (skinName.Length != 0)
            {
                SkinChange((SkinKind)i, skinName);
                SetColor(GameManager.instance.userInfoManager.GetUserEqip((SkinKind)i).skinName, GameManager.instance.userInfoManager.GetUserEqip((SkinKind)i).color_01, 1);
                SetColor(GameManager.instance.userInfoManager.GetUserEqip((SkinKind)i).skinName, GameManager.instance.userInfoManager.GetUserEqip((SkinKind)i).color_02, 2);
            }
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
            color = GetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF).skinName , 1);
        }
        if (slotName.Contains("haB") && slotNum == 2)
        {
            color = GetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF).skinName, 2);
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
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB).skinName, color);
        }
        if (slotName.Contains("haF") && slotNum == 2)
        {
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB).skinName, color);
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
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains(skinKind.ToString()))
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
