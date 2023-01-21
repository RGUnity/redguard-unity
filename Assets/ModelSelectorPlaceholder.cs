using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RGFileImport;
public class ModelSelectorPlaceholder : MonoBehaviour
{
    List<string> GetModelNames()
    {
        string[] names = System.Enum.GetNames(typeof(RGFileImport.RGFiles));
        return new List<string>(names); 
    }
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
        optionCheck = GetComponent<TMP_Dropdown>();
        List<string> modelOptions = GetModelNames();
        
        optionCheck.ClearOptions();
        optionCheck.AddOptions(modelOptions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
