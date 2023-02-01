using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerStartPosition;
    
    [Header("Dynamic Variables")]
    public static bool isGamePaused;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (playerStartPosition != null)
        {
            SetPlayerStartPosition(playerStartPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PauseGame()
    {
        isGamePaused = true;
        print("Game Paused");
    }
    
    public static void ContinueGame()
    {
        isGamePaused = false;
        print("Game Continues");
    }

    private void SetPlayerStartPosition(Transform pos)
    {
        var positionOffset = new Vector3(0, 1, 0);
        var newPosition = playerStartPosition.position + positionOffset;
        var newRotation = playerStartPosition.rotation;
        player.transform.SetPositionAndRotation(newPosition, newRotation);
    }
}
