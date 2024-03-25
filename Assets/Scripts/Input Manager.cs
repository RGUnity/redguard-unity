using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PauseMenuLoader _pauseMenuLoader;

    public Vector2 move;
    public bool jump;
    public bool moveModifier;
    public bool use;
    
    // Input System Actions
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction moveModifierAction;
    private InputAction useAction;
    
    private UIWindowManager UiWindowManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        UiWindowManager = FindFirstObjectByType<UIWindowManager>();
        
        // Find input actions
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        moveModifierAction = InputSystem.actions.FindAction("Move Modifier");
        useAction = InputSystem.actions.FindAction("Use");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Quick Save"))
        {
            print("Input Button Called: Quick Save");
            GameSaver.QuickSave();
        }
        if (Input.GetButtonDown("Quick Load"))
        {
            print("Input Button Called: Quick Load");
            GameLoader.QuickLoad();
        }
        

        if (!Game.Menu.isLoadedAdditively)
        {
            // Inputs that can only happen when the additive pause menu is HIDDEN 

            if (Input.GetButtonDown("Fire2"))
            {
                UiWindowManager.ToggleInventory();
            }

            if (Input.GetButtonDown("Pause"))
            {
                _pauseMenuLoader.ShowMainMenu();
            }
        }
        
        // Player Character Controls
        move = moveAction.ReadValue<Vector2>();
        jump = jumpAction.WasPressedThisFrame();
        moveModifier = moveModifierAction.IsPressed();
        use = useAction.WasPressedThisFrame();
    }
}
