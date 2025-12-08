using System;
using UnityEngine;

public static class MainUIScript
{
    static MainUIScript()
    {
    }

    static public void SetActivateText(string text)
    {
        // TODO: don't set when RTX subtitle are shown (?)
        Game.uiManager.ShowInteractionText(text);
    }
}
