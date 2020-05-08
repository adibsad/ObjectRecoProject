using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenPos : MonoBehaviour
{
    public GameObject obj;
    Renderer rend;

    Vector3[] corners = new Vector3[8];
    float[] x_pixels = new float[8];
    float[] y_pixels = new float[8];
    private Vector2 topLeft;
    private Vector2 bottomRight;

    float degrees = 0.0f;
    int increment = 45;
    int numberOfImages = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        RecordImages();

    }

    void RecordImages()
    {
        Vector3 rotation = new Vector3(0, 0, 0);


        rend = obj.GetComponent<Renderer>();
        PopulateCorners(rend.bounds);

       
        string csvPath = initFile();
        

        while (numberOfImages < 1)
        {
            rotation.y += increment;

            degrees += rotation.y;

            if (degrees > 360)
            {
                rotation.x += 45;
                degrees = 0;
            }

            transform.Rotate(rotation);
            //New origin or anchor point
            Vector3 pivot = transform.position;

            for (int i = 0; i < corners.Length; i++)
            {
                //Original corner position
                Vector3 org = corners[i];

                //Direction vector from new origin to corner position
                Vector3 dir = org - pivot;

                //Rotated dir about real origin
                dir = Quaternion.Euler(rotation) * dir;

                //Add direction to anchor point
                Vector3 rotated_org = dir + pivot;

                corners[i] = rotated_org;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(rotated_org);

                x_pixels[i] = screenPos.x;
                y_pixels[i] = screenPos.y;

            }

            topLeft = new Vector2(Mathf.Min(x_pixels), Screen.height - Mathf.Max(y_pixels));
            bottomRight = new Vector2(Mathf.Max(x_pixels), Screen.height - Mathf.Min(y_pixels));
            //Debug.Log(topLeft + ", " + bottomRight);
            
            string path = ScreenShot.takeScreenshot(Camera.main, 720, 480, numberOfImages);

            string[] dataLine = { $"{numberOfImages} {path} truck train {topLeft.x} {topLeft.y} {bottomRight.x} {bottomRight.y}" };
            numberOfImages++;

            try
            {
                File.AppendAllLines(csvPath, dataLine);
            }
            catch (IOException e)
            {
                Debug.Log(e);
            }
        }

        
    }

    string initFile()
    {
        string path = ScreenShot.getPath() + "\\training_labels.csv";

        string header = "index path label eval x_1 y_1 x_2 y_2\r\n";

        try
        {
            File.WriteAllText(path, header);
        }
        catch(IOException e)
        {
            Debug.Log(e);
        }

        return path;
    }
    /*
    void Update()
    {

        transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
        degrees += speed * Time.deltaTime;

        if (degrees > 360)
        {
            transform.Rotate(45, 0, 0);
            Debug.Log(transform.eulerAngles);
            degrees = 0;
        }
        //numberOfImages++;

        //Debug.Log(numberOfImages);

    }*/

    // Fill our scratchpad array with the corners of an axis-aligned bounding box.  
    void PopulateCorners(Bounds bounds)
    {

        corners[0] = bounds.min;
        corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        corners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        corners[4] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        corners[7] = bounds.max;
    }

  
}
