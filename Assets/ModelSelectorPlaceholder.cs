using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ModelSelectorPlaceholder : MonoBehaviour
{
    TMP_Dropdown optionCheck;
    [SerializeField] RGFiles selectedModel; 
    // Start is called before the first frame update
    public void ChangeModelNow()
    {  
        
        
        FindObjectOfType<ModelUnityReader>().SetModel((RGFiles)optionCheck.value);
    }

    void OnValueChanged()
    {
        ChangeModelNow();
    }
    void Awake()
    {
        List<string> modelOptions = new List<string>(new string[2]);
        
        optionCheck.ClearOptions();
        optionCheck.AddOptions(modelOptions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
