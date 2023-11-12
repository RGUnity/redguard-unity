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
        GameSaver.SaveGame(1);
    }
    public void SaveSlot2()
    {
        GameSaver.SaveGame(2);
    }
    public void SaveSlot3()
    {
        GameSaver.SaveGame(3);
    }
}
