using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Todo: use this to encode time into savefile
        //print(DateTime.Now.ToBinary().GetType());
    }

    public void SaveGame()
    {

    }


    public void SaveSlot1()
    {
        var gameSaver = FindObjectOfType<GameSaver>();
        gameSaver.SaveGame(1);
    }
    public void SaveSlot2()
    {
        var gameSaver = FindObjectOfType<GameSaver>();
        gameSaver.SaveGame(2);
    }
    public void SaveSlot3()
    {
        var gameSaver = FindObjectOfType<GameSaver>();
        gameSaver.SaveGame(3);
    }
}
