using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Spine.Unity;

public class ScriptController : MonoBehaviour
{
    public static ScriptController instace;

    public GameObject donClickPannel;
    public GameObject all;
    public Transform scriptPannel;
    public Transform selectPannel;

    public SkeletonAnimation skeletonAnimation;


    private void Start()
    {
        instace = this;
    }

    [ContextMenu("테스트")]
    public void Test()
    {
    
    }

    public void ScriptOpen()
    {
        donClickPannel.SetActive(true);
        all.SetActive(true);
    }
    public void ScriptClose()
    {
        all.transform.localPosition = new Vector2(all.transform.localPosition.x , 0);
        donClickPannel.SetActive(false);
        all.SetActive(false);

        if (skeletonAnimation.AnimationName != GameManager.instance.userInfoManager.currentAnimation)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, GameManager.instance.userInfoManager.currentAnimation, true);
        }
    }
    public void TextScriptPlay(int code)
    {
        ScriptOpen();

        if (code == 0)
        {
            ScriptClose();
            return;
        }

        all.transform.DOLocalMoveY(0, 0.2f);

        tempTextScriptPlayCoroutine = TextScriptPlayCoroutine(code);
        StartCoroutine(tempTextScriptPlayCoroutine);
    }

    IEnumerator tempTextScriptPlayCoroutine;
    IEnumerator TextScriptPlayCoroutine(int code)
    {
        string name = GameManager.instance.scriptManager.GetscriptInfo(code).name;
        string script = GameManager.instance.scriptManager.GetscriptInfo(code).script;
        string aniName = GameManager.instance.scriptManager.GetscriptInfo(code).aniName;

        if (!aniName.Contains("Nul"))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, aniName, true);
        }
        else
        {
            skeletonAnimation.AnimationState.SetAnimation(0, GameManager.instance.userInfoManager.currentAnimation, true);
        }

        scriptPannel.Find("이름").GetChild(0).GetComponent<Text>().text = name;
        if (name == "Character01")
        {
            scriptPannel.Find("이름").GetComponent<Image>().color = new Color(255 / 255f, 31 / 255f, 102 / 255f, 1);
        }
        else
        {
            scriptPannel.Find("이름").GetComponent<Image>().color = new Color(20 / 255f, 41 / 255f, 159 / 255f, 1);
        }

        Text text = scriptPannel.Find("내용").GetChild(0).GetComponent<Text>();
        text.text = "";
        Button btn = scriptPannel.Find("내용").GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => { TextScriptSet(code); });

        char[] charListScript = script.ToCharArray();
        float scriptTime = 0.05f;
        WaitForSeconds waitTime = new WaitForSeconds(scriptTime);
        for (int i = 0; i < charListScript.Length; i++)
        {
            if (charListScript[i] =='=')
            {
                charListScript[i] = '\n';
            }
            text.text += charListScript[i].ToString();
            yield return waitTime;
        }

        TextScriptSet(code);
    }

    void TextScriptSet(int code)
    {
        if (tempTextScriptPlayCoroutine != null)
        {
            StopCoroutine(tempTextScriptPlayCoroutine);
        }

        string script = GameManager.instance.scriptManager.GetscriptInfo(code).script;
        script = script.Replace('=', '\n');
        int scriptCode = GameManager.instance.scriptManager.GetscriptInfo(code).nextCode;
        string[] selectScript = GameManager.instance.scriptManager.GetscriptInfo(code).selectScript;
        int[] selectCode = GameManager.instance.scriptManager.GetscriptInfo(code).selectCode;

        Text text = scriptPannel.Find("내용").GetChild(0).GetComponent<Text>();
        Button btn = scriptPannel.Find("내용").GetComponent<Button>();
        text.text = script;

        btn.onClick.RemoveAllListeners();

        if (scriptCode != 0)
        {
            all.transform.DOLocalMoveY(0, 0.3f);
            btn.onClick.AddListener(() => { TextScriptPlay(scriptCode); });
            return;
        }

        int selectLength = selectScript.Length;
        int movePosY = 95;
        all.transform.DOLocalMoveY((movePosY * selectLength), 0.3f);

        for (int i = 0; i < selectLength; i++)
        {
            Text selectText = selectPannel.GetChild(i).GetChild(0).GetComponent<Text>();
            Button selectBtn = selectPannel.GetChild(i).GetComponent<Button>();
           
            selectText.text = selectScript[i];
            selectBtn.onClick.RemoveAllListeners();
            int index = i;
            selectBtn.onClick.AddListener(() => { TextScriptPlay(selectCode[index]); });
        }

        if (selectLength == 0)
        {
            btn.onClick.AddListener(() => { ScriptClose(); });
        }
    }

}
