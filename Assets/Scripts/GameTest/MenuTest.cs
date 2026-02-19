using UnityEngine;
using UnityEngine.UI;
using RGFileImport;
using System.Collections.Generic;

public class MenuTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*
    RGINIFile iniData;
    Canvas canvas;
    List<GameObject> textObjects;
    List<Text> textContents;
    GameObject text1;
    Text text1text;
    void Start()
    {
        iniData = new RGINIFile();
        iniData.LoadFile($"{Game.pathManager.GetRootFolder()}/MENU.INI");
    }
    */

    // Update is called once per frame
    void Start()
    {
        GameObject myText;
        Canvas myCanvas;
        Text text;
        RectTransform rectTransform;

        // Canvas
        gameObject.name = "TestCanvas";
        gameObject.AddComponent<Canvas>();

        myCanvas = gameObject.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        // Text
        myText = new GameObject();
        myText.transform.parent = gameObject.transform;
        myText.name = "wibble";

        text = myText.AddComponent<Text>();
        text.font = (Font)Resources.Load("ibm-plex-mono-cyrillic-100-normal");
        text.text = "wobble";
        text.fontSize = 100;

        // Text position
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(400, 200);
    }
    void Update()
    {
        
    }


}
