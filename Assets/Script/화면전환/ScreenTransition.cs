using UnityEngine;
using DG.Tweening;

public class ScreenTransition : MonoBehaviour
{
    [Header("카메라")]
    public Camera characterCamera;
    public Camera uiCamera;
    float cameraMoveSpeed = 0.3f;
    [Header("그림자")]
    public Transform shadow;

    [Space]

    [Header("메뉴")]
    public Transform leftPannel;
    public GameObject minigameIcon;
    public GameObject photoIcon;
    public GameObject clothIcon;

    [Space]

    [Header("옷장")]
    public Transform clothPannel;
    public GameObject clothBtn;
    public GameObject backBtn;


    float uiCamMoveX = 14.5f;
    float characterCamMoveX = .8f;
    float shadowMoveX = .8f;

    public void ClothOpen()
    {
        clothBtn.SetActive(false);
        backBtn.SetActive(false);
        characterCamera.transform.DOMoveX(characterCamMoveX, cameraMoveSpeed);
        uiCamera.transform.DOMoveX(uiCamMoveX, cameraMoveSpeed);
        shadow.transform.DOMoveX(shadowMoveX, cameraMoveSpeed);
    }
}
