using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PauseMenuLoader _pauseMenuLoader;
    
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
    private InputAction _dropDownAction;
    private InputAction _climbUpAction;
    
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
        _dropDownAction = InputSystem.actions.FindAction("Drop Down");
        _climbUpAction = InputSystem.actions.FindAction("Climb Up");
    }

    // Update is called once per frame
    void Update()
    {
        // Read input signals
        Game.Input.move = _moveAction.ReadValue<Vector2>();
        Game.Input.moveModifier = _moveModifierAction.IsPressed();
        
        Game.Input.use = _useAction.WasPressedThisFrame();
        
        // jump is used in FixedUpdate. To avoid sync issues, PlayerMovement.cs sets it to false after processing it.
        if (_jumpAction.WasPressedThisFrame())
        {
            Game.Input.jump = true;
        }
        
        Game.Input.toggleInventory = _toggleInventoryAction.WasPressedThisFrame();
        Game.Input.toggleMap = _toggleMapAction.WasPressedThisFrame();
        Game.Input.quickSave = _quickSaveAction.WasPressedThisFrame();
        Game.Input.quickLoad = _quickLoadAction.WasPressedThisFrame();
        Game.Input.dropDown = _dropDownAction.WasPressedThisFrame();
        Game.Input.climbUp = _climbUpAction.WasPressedThisFrame();
        
        if (Game.Input.quickSave)
        {
            print("Input Button Called: Quick Save");
            GameSaver.QuickSave();
        }
        if (Game.Input.quickLoad)
        {
            print("Input Button Called: Quick Load");
            GameLoader.QuickLoad();
        }
        
        if (!Game.Menu.isLoadedAdditively)
        {
            // Inputs that can only happen when the additive pause menu is HIDDEN 

            if (Game.Input.toggleInventory)
            {
                UiWindowManager.ToggleInventory();
            }

            if (Game.Input.togglePauseMenu)
            {
                _pauseMenuLoader.ShowMainMenu();
            }
        }
    }
}
