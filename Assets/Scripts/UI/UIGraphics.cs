using UnityEngine;

public class UIGraphics: MonoBehaviour
{
    bool loadScreenActive;
    Texture loadScreenTexture;

    void Start()
    {
        loadScreenActive = false;
    }

    void OnGUI()
    {
        if(loadScreenActive)
        {
            Rect fsRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(fsRect,loadScreenTexture, ScaleMode.ScaleToFit);
        }
    }

    public void ShowLoadingScreen(Texture t)
    {
        loadScreenActive = true;
        loadScreenTexture = t;
    }
    public void HideLoadingScreen()
    {
        loadScreenActive = false;
    }

}
