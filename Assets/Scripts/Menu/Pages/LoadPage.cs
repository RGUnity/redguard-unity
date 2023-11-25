using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPage : GenericMenuPage
{
    // OnEnable() is used by the parent object, "GenericMenuPage"
    
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
