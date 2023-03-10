using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

public class SavePage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Todo: use this to encode time into savefile
        // print(DateTime.Now.ToBinary());
    }

    public void SaveGame()
    {

    }


    public void SaveSlot1()
    {
        DataSerializer.ChangeProfile(1);
        var gameSaver = FindObjectOfType<GameSaver>();
        gameSaver.SaveGame();
    }
    public void SaveSlot2()
    {
        DataSerializer.ChangeProfile(2);
        var gameSaver = FindObjectOfType<GameSaver>();
        gameSaver.SaveGame();
    }
    public void SaveSlot3()
    {
        DataSerializer.ChangeProfile(3);
        var gameSaver = FindObjectOfType<GameSaver>();
        gameSaver.SaveGame();
    }
}
