using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPageListItem : GenericSaveFileListItem
{
    public void RequestFileLoad()
    {
        GameLoader.LoadGame(saveFileName);
    }
}
