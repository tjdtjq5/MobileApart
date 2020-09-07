using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScriptController : MonoBehaviour
{
    public GameObject donClickPannel;
    public GameObject all;
    public Transform scriptPannel;
    public Transform selectPannel;


    [ContextMenu("테스트")]
    public void Test()
    {
        ScriptOpen();
        TextScriptPlay(1);
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
    }
    void TextScriptPlay(int code)
    {
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

        scriptPannel.Find("이름").GetChild(0).GetComponent<Text>().text = name;

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
        string[] selectScript = GameManager.instance.scriptManager.GetscriptInfo(code).selectScript;
        int[] selectCode = GameManager.instance.scriptManager.GetscriptInfo(code).selectCode;

        Text text = scriptPannel.Find("내용").GetChild(0).GetComponent<Text>();
        Button btn = scriptPannel.Find("내용").GetComponent<Button>();
        text.text = script;

        btn.onClick.RemoveAllListeners();

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
