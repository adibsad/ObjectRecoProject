using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    public int width = 720;
    public int height = 480;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        Screen.SetResolution(width, height, true, 60);
        
        Debug.Log(Screen.currentResolution);

    }

}
