using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot : MonoBehaviour
{
    public int captureWidth;
    public int captureHeight;

    //private int numberOfImages = 0;

    private RenderTexture renderTexture;
    private Texture2D screenshot;

    public static string getPath() { return Path.Combine(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), "Screenshots"); }

    static string getPath(int width, int height, int count)
    {

        string path = Path.Combine(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), "Screenshots");

        System.IO.Directory.CreateDirectory(path);
        string filename = string.Format("{0}\\screenshot_{1}x{2}_{3}.png", path, width, height, count);

        return filename;
    }

    public static string takeScreenshot(Camera camera, int width, int height, int numberOfImages)
    {
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        //Camera image renders on to target rendertexture
        camera.targetTexture = renderTexture;
        camera.Render();

        //Sets active RenderTexture from which ReadPixels will load on to Texture2D 
        RenderTexture.active = renderTexture;

        //ReadPixel(new Rect, width, hegith, destX, destY, mimmapRecalc)
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0, false);
        screenshot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null;

        Destroy(renderTexture);

        byte[] screenShot = screenshot.EncodeToPNG();

        string path = getPath(width, height, numberOfImages);

        try
        {
            System.IO.File.WriteAllBytes(path, screenShot);
        }
        catch(IOException e) { Debug.Log(e); }

        Debug.Log(string.Format("Took screenshot to: {0}", path));

        return path;

    }


}