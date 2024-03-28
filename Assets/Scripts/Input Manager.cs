using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PauseMenuLoader _pauseMenuLoader;

    public Vector2 move;
    public bool moveModifier;
    public bool use;
    public bool jump;
    public bool toggleInventory;
    public bool toggleMap;
    public bool quickSave;
    public bool quickLoad;
    public bool togglePauseMenu;
    
    // Input System Actions
    private InputAction _moveAction;
    private InputAction _moveModifierAction;
    
    private InputAction _useAction;
    private InputAction _jumpAction;
    private InputAction _toggleInventoryAction;
    private InputAction _toggleMapAction;
    private InputAction _quickSaveAction;
    private InputAction _quickLoadAction;
    private InputAction _togglePauseMenuAction;
    
    private UIWindowManager UiWindowManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        UiWindowManager = FindFirstObjectByType<UIWindowManager>();
        
        // Find input actions
        _moveAction = InputSystem.actions.FindAction("Move");
        _moveModifierAction = InputSystem.actions.FindAction("Move Modifier");
        
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _useAction = InputSystem.actions.FindAction("Use");
        _toggleInventoryAction = InputSystem.actions.FindAction("Toggle Inventory");
        _toggleMapAction = InputSystem.actions.FindAction("Toggle Map");
        _quickSaveAction = InputSystem.actions.FindAction("Quick Save");
        _quickLoadAction = InputSystem.actions.FindAction("Quick Load");
        _togglePauseMenuAction = InputSystem.actions.FindAction("Toggle Pause Menu");
    }

    // Update is called once per frame
    void Update()
    {
        // Read input signals
        move = _moveAction.ReadValue<Vector2>();
        moveModifier = _moveModifierAction.IsPressed();
        
        use = _useAction.WasPressedThisFrame();
        
        // jump is used in FixedUpdate. To avoid sync issues, PlayerMovement.cs sets it to false after processing it.
        if (_jumpAction.WasPressedThisFrame())
        {
            jump = true;
        }
        
        toggleInventory = _toggleInventoryAction.WasPressedThisFrame();
        toggleMap = _toggleMapAction.WasPressedThisFrame();
        quickSave = _quickSaveAction.WasPressedThisFrame();
        quickLoad = _quickLoadAction.WasPressedThisFrame();
        
        if (quickSave)
        {
            print("Input Button Called: Quick Save");
            GameSaver.QuickSave();
        }
        if (quickLoad)
        {
            print("Input Button Called: Quick Load");
            GameLoader.QuickLoad();
        }
        
        if (!Game.Menu.isLoadedAdditively)
        {
            // Inputs that can only happen when the additive pause menu is HIDDEN 

            if (toggleInventory)
            {
                UiWindowManager.ToggleInventory();
            }

            if (togglePauseMenu)
            {
                _pauseMenuLoader.ShowMainMenu();
            }
        }
    }
}
