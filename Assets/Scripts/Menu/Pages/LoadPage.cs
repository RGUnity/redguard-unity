using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPage : MonoBehaviour
{
    public void LoadSlot1()
    {
        Game.EnterSceneMode = EnterSceneModeEnum.Load;
        
        GameLoader.LoadGame(1);
    }
    public void LoadSlot2()
    {
        Game.EnterSceneMode = EnterSceneModeEnum.Load;
        
        GameLoader.LoadGame(2);
    }
    public void LoadSlot3()
    {
        Game.EnterSceneMode = EnterSceneModeEnum.Load;
        
        GameLoader.LoadGame(3);
    }
}
