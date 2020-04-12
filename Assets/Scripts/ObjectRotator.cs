using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public int captureWidth;
    public int captureHeight;
    public float speed;

    private int numberOfImages = 0;
    private float degrees = 0;

    public GameObject target;
    private RenderTexture renderTexture;
    private Texture2D screenshot;

    void Start() {
        //transform.Rotate(90, 0, 0);

        //while (numberOfImages < 3) {
        // transform.Rotate(0, 10, 0);

        // takeScreenshot();
        //}


        //axis = Vector3.up;
        
        
    }

    // Update is called once per frame
   void Update() {

        transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
        degrees += speed * Time.deltaTime;

        if (degrees > 360) {
            transform.Rotate(45, 0, 0);
            Debug.Log(transform.eulerAngles);
            degrees = 0;
        }
        numberOfImages++;

        Debug.Log(numberOfImages);
        
    } 
   
   public string getPath(int width, int height, int count) {
       
       string path = Path.Combine(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), "Screenshots");

       System.IO.Directory.CreateDirectory(path);
       string filename = string.Format("{0}/screenshot_{1}x{2}_{3}.png", path, width, height, count);

       return filename;
   }

   public void takeScreenshot() {
       renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
       screenshot = new Texture2D(captureWidth, captureWidth, TextureFormat.RGB24, false);

       Camera camera = Camera.main;
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

       string path = getPath(captureWidth, captureHeight, numberOfImages);

       System.IO.File.WriteAllBytes(path, screenShot);
       ++numberOfImages;
       Debug.Log(string.Format("Took screenshot to: {0}", path));


   }

    
}
