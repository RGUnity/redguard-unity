using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUIPopup : GenericUIWindow
{
    // This is intended as a string that can contain different kinds of information, ...
    // ... Like a path to a savefile, or the name of a new savefile
    [SerializeField] public string infoString;
}
