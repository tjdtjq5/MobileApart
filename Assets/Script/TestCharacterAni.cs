using Spine;
using Spine.Unity;
using UnityEngine;

public class TestCharacterAni : MonoBehaviour
{
    public string AnimationString;

    SkeletonAnimation sa;

    private void Start()
    {
        sa = this.GetComponent<SkeletonAnimation>();
    }

    [ContextMenu("애니실행")]
    public void AniStart()
    {
        sa.AnimationState.SetAnimation(0, AnimationString, false);
    }
}
