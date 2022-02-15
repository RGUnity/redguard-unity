using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class API_Checker : MonoBehaviour
{
    public Text textfield;
    // Start is called before the first frame update
    void Start()
    {
        GraphicsDeviceType currentAPI = SystemInfo.graphicsDeviceType;
        textfield.text = currentAPI.ToString();
        if (
        (currentAPI == GraphicsDeviceType.Direct3D12) ||
        (currentAPI == GraphicsDeviceType.Vulkan) ||
        (currentAPI == GraphicsDeviceType.Metal))
        {
            textfield.color = new Color(0, 0.5f, 0, 100);
        }
        else
        {
            textfield.color = new Color(0.5f, 0, 0, 100);
        }

    }

}
