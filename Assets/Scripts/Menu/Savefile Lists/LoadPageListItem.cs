using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPageListItem : GenericSaveFileListItem
{
    public void RequiestFileLoad()
    {
        GameLoader.LoadGame(saveFileName);
    }
}
