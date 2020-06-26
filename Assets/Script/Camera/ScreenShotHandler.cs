using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotHandler : MonoBehaviour
{
    public static ScreenShotHandler instance;
    private Camera myCamera;
    bool takeScreenshotOnNextFrame;

    string path;
    string fileName = "ScreenShot_";
    int currentNum;

    public Image screenshot_img;
  

    private void Awake()
    {
        instance = this;
        myCamera = GetComponent<Camera>();
        path = Path.Combine(PathForDocumentsFile(""), fileName);
    }

    // 사진 찍고 저장
    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0,0);


            byte[] byteArray = renderResult.EncodeToPNG();

            // 미리보기 창 
            screenshot_img.sprite = ScreenshotImg(byteArray);

            myCamera.targetTexture = null;

            // 저장 

            string saveGameFileName = fileName + currentNum.ToString() + ".png";
            string filePath = Path.Combine(path, saveGameFileName);

            //Create Directory if it does not exist
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            System.IO.File.WriteAllBytes(filePath, byteArray);

            Debug.Log("Screenshot Succecs");
            RenderTexture.ReleaseTemporary(renderTexture);
        }
    }
    private void TakeScreenshot(int width, int height, int num = 0)
    {
        currentNum = num;
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }

    public static void TakeScreenshot_Static(int width, int height, int num = 0)
    {
        instance.TakeScreenshot(width, height, num);
    }

    int testNum = 0;
    public void TestScreenshot()
    {
        TakeScreenshot(Screen.width, Screen.height, testNum);
        testNum++;
    }

    //찍은 이미지를 불러오기
    public Sprite ScreenshotImg(byte[] byteArray)
    {
        RenderTexture renderTexture = myCamera.targetTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);

        renderResult.LoadImage(byteArray);
        Sprite screenshotSprite = Sprite.Create(renderResult, new Rect(0, 0, renderTexture.width, renderTexture.height), new Vector2(0.5f, 0.5f));
        return screenshotSprite;
    }

    //저장된 이미지 찾기 
    public Sprite SystemIOFileLoad(int num)
    {
        string saveGameFileName = fileName + num.ToString()+ ".png";
        string pathAndFile = Path.Combine(path, saveGameFileName);

        if (!File.Exists(pathAndFile))
        {
            return null;
        }
        byte[] byteTexture = System.IO.File.ReadAllBytes(pathAndFile);
        Texture2D texture = new Texture2D(0, 0);
        if (byteTexture.Length > 0) {  texture.LoadImage(byteTexture); }
        Sprite screenshotSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return screenshotSprite;
    }

    public string PathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }else if(Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }


 
}
