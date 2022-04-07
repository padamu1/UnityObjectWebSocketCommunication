using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCheck : MonoBehaviour
{
    public int iFont_Size;
    public int iFPS_Limit;

    float deltaTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        Application.targetFrameRate = iFPS_Limit;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / iFont_Size;
        style.normal.textColor = new Color(255, 255, 255, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / Time.deltaTime;
        string text = string.Format("{0:0.0}ms({1:0.}fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
