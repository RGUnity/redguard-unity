using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class DPIScaler : MonoBehaviour
{
    [SerializeField] private CanvasScaler scaler;
    
    [SerializeField] private float baseDPI = 96f;

    private void Awake()
    {
        scaler.scaleFactor = GetScaleFactor();
    }

    public float GetScaleFactor()
    {
        float currentDPI = Screen.dpi > 0 ? Screen.dpi : baseDPI;
        return currentDPI / baseDPI;;
    }
}
