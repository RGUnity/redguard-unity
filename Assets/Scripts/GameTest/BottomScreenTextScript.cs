using UnityEngine;

public class BottomScreenTextScript : MonoBehaviour
{
    bool frameTextSet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!frameTextSet) 
            GetComponent<TMPro.TextMeshProUGUI>().text = "";
        else
            frameTextSet = false;
    }

    public void SetText(string text)
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = text;
        frameTextSet = true;
    }
}
