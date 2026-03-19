using TMPro;
using UnityEngine;

public class LocalUIData : MonoBehaviour
{
    [SerializeField] public DialogueOption optionPrefab;
    [SerializeField] public Transform optionsParent;
    [SerializeField] public TMP_Text interactionTextDisplay;
    [SerializeField] public TMP_Text subtitleTextDisplay;
    [SerializeField] public UIGraphics uiGraphics;
}
