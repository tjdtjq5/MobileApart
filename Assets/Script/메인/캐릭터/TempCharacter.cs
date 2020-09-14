using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCharacter : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    List<string> skinList = new List<string>();

    private void Awake()
    {
        skeletonAnimation = this.GetComponent<SkeletonAnimation>();
    }

    [ContextMenu("테승트")]
    public void Test()
    {
        CharacterSet(GameManager.instance.userInfoManager.GetUserEqip());
    }

    public void CharacterSet(List<UserSkin> userSkinList)
    {
        skinList.Clear();
        for (int i = 0; i < userSkinList.Count; i++)
        {
            SkinChange(userSkinList[i].skinName);
            SetColor(userSkinList[i].skinName, userSkinList[i].color_01, 1);
            SetColor(userSkinList[i].skinName, userSkinList[i].color_02, 2);
        }
    }

    public void SkinChange(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains(skinName))
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

    public void SetColor(string slotName, Color color, int slotNum = 1)
    {
        if (color == null)
        {
            return;
        }
        if (color == Color.clear)
        {
            float randR = (float)Random.RandomRange(0, 255) / 255;
            float randG = (float)Random.RandomRange(0, 255) / 255;
            float randB = (float)Random.RandomRange(0, 255) / 255;
            color = new Color(randR, randG, randB, 1);
        }

        if (slotName.Contains("haB") && slotNum == 1)
        {
            color = GetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF)[0].skinName, 1);
        }
        if (slotName.Contains("haB") && slotNum == 2)
        {
            color = GetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haF)[0].skinName, 2);
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
                    }
                }

            }
        }

        if (slotName.Contains("haF") && slotNum == 1)
        {
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB)[0].skinName, color);
        }
        if (slotName.Contains("haF") && slotNum == 2)
        {
            SetColor(GameManager.instance.userInfoManager.GetUserEqip(SkinKind.haB)[0].skinName, color);
        }

        return;
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

}
