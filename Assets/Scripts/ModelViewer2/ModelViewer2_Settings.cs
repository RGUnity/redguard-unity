using UnityEngine;

public class ModelViewer2_Settings : MonoBehaviour
{
    [SerializeField] private ModelViewer2 mv2;
    [SerializeField] private ModelViewer2_GUI mv2_gui;
    [SerializeField] private ModelViewer2_Camera mv2_camera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ToggleFlyMode(bool toggle)
    {
        mv2_camera.useFlyMode = toggle;
    }
}
