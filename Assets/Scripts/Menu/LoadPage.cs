using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

public class LoadPage : MonoBehaviour
{
    public void LoadSlot1()
    {
        DataSerializer.ChangeProfile(1);
        var gameLoader = FindObjectOfType<GameLoader>();
        gameLoader.LoadGame();
    }
    public void LoadSlot2()
    {
        DataSerializer.ChangeProfile(2);
        var gameLoader = FindObjectOfType<GameLoader>();
        gameLoader.LoadGame();
    }
    public void LoadSlot3()
    {
        DataSerializer.ChangeProfile(3);
        var gameLoader = FindObjectOfType<GameLoader>();
        gameLoader.LoadGame();
    }
}
