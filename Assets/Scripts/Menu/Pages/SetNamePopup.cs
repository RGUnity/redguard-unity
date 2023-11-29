using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetNamePopup : GenericUIWindow
{
    [SerializeField] public TMP_InputField inputField;

    protected override void OnEnableChild()
    {
        inputField.text = "";
    }
}
