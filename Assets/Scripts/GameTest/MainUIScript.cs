using System;
using UnityEngine;

public static class MainUIScript
{
    static BottomScreenTextScript bottomScreenText;

    static MainUIScript()
    {
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag("BottomScreenText");
        bottomScreenText = textObjects[0].GetComponent<BottomScreenTextScript >();
    }

    static public void SetActivateText(string text)
    {
        // TODO: don't set when RTX subtitle are shown (?)
        bottomScreenText.SetText(text);
    }
}
