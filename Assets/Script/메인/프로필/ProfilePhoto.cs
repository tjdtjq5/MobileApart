using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePhoto : MonoBehaviour
{
    public Image tempSprite;

    private Camera myCamera;
    bool takeScreenshotOnNextFrame;


    private void Awake()
    {
        myCamera = GetComponent<Camera>();
    }

    // 사진 찍고 저장
    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(400, 400, TextureFormat.ARGB32, false);
            Rect rect = new Rect(310, 140, 400, 400);
            renderResult.ReadPixels(rect, 0, 0);


            byte[] byteArray = renderResult.EncodeToPNG();

            tempSprite.sprite = ScreenshotImg(byteArray);

            SaveProfileImg(byteArray);

            myCamera.targetTexture = null;

        }
    }

    void SaveProfileImg(byte[] bytes)
    {
        Param data = new Param();
        data.Add("ProfileImg", bytes);

        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPublicContents, "Profile", (callback) =>
        {


        });

        BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "Profile" , "" , data, (callback) =>
        {

       
        });
    }

    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }

    public void ProfileTakePhoto()
    {
        TakeScreenshot(Screen.width, Screen.height);
    }

    public Sprite ScreenshotImg(byte[] byteArray)
    {
        RenderTexture renderTexture = myCamera.targetTexture;
        Texture2D renderResult = new Texture2D(400, 400, TextureFormat.ARGB32, false);
        Rect rect = new Rect(310, 140, 400, 400);
        renderResult.ReadPixels(rect, 0, 0);

        renderResult.LoadImage(byteArray);
        Sprite screenshotSprite = Sprite.Create(renderResult, new Rect(0, 0, 400, 400), new Vector2(0.5f, 0.5f));
        return screenshotSprite;
    }
}
