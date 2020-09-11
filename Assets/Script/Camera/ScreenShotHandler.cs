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

    IEnumerator[] LoadCoroutineList = new IEnumerator[100];
    [HideInInspector] public Sprite[] loadSprite = new Sprite[100];
    //저장된 이미지 찾기 
    public void SystemIOFileLoad(int num, System.Action callback)
    {
        /*
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
        */

        if (LoadCoroutineList[num] != null)
        {
            StopCoroutine(LoadCoroutineList[num]);
        }
        LoadCoroutineList[num] = LoadCoroutine(num, () => { callback(); });
        StartCoroutine(LoadCoroutineList[num]);
    }

    IEnumerator LoadCoroutine(int num, System.Action callback)
    {
        string saveGameFileName = fileName + num.ToString() + ".png";
        string pathAndFile = Path.Combine(path, saveGameFileName);

        if (!File.Exists(pathAndFile))
        {
            yield break;
        }
        byte[] byteTexture = System.IO.File.ReadAllBytes(pathAndFile);
        Texture2D texture = new Texture2D(0, 0);
        if (byteTexture.Length > 0) { texture.LoadImage(byteTexture); }
        loadSprite[num] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        yield return null;

        callback();
    }

    public void AllStopLoad()
    {
        for (int i = 0; i < LoadCoroutineList.Length; i++)
        {
            if (LoadCoroutineList[i] != null)
            {
                StopCoroutine(LoadCoroutineList[i]);
            }
        }
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

    // 사진 제거 
    public void Remove(int index)
    {
        string saveGameFileName = fileName + index.ToString() + ".png";
        string pathAndFile = Path.Combine(path, saveGameFileName);

        Debug.Log(pathAndFile);
        if (!File.Exists(pathAndFile))
        {
            return;
        }
        Debug.Log(pathAndFile);
        File.Delete(pathAndFile);

        int tempIndex = index;
        while (true)
        {
            tempIndex = tempIndex + 1;
            saveGameFileName = fileName + tempIndex.ToString() + ".png";
            pathAndFile = Path.Combine(path, saveGameFileName);
            if (!File.Exists(pathAndFile))
            {
                break;
            }
            int moveIndex = tempIndex - 1;
            string moveFileName = fileName + moveIndex.ToString() + ".png";
            string movePathAndFile = Path.Combine(path, moveFileName);
            File.Move(pathAndFile, movePathAndFile);
        }
    }

    public int LastIndex()
    {
        int index = 0;
        while (true)
        {
            string saveGameFileName = fileName + index.ToString() + ".png";
            string pathAndFile = Path.Combine(path, saveGameFileName);
            if (!File.Exists(pathAndFile))
            {
                return index;
            }
            index++;
        }
    
    }
}
