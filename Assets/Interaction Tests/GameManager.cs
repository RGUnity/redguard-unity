using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Config")]

    [Header("Dynamic Variables")]
    public static bool isGamePaused;
    
    
    // Start is called before the first frame update
    void Start()
    {
        isGamePaused = false;
    }

    public static void PauseGame()
    {
        isGamePaused = true;
        //print("Game Paused");
    }
    
    public static void ContinueGame()
    {
        isGamePaused = false;
        //print("Game Continues");
    }
}
