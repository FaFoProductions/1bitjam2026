using UnityEngine;
using UnityEngine.InputSystem;
public class InputManagerSingleton : MonoBehaviour
{
    public static InputManagerSingleton Instance { get; private set; }
    public PlayerControls playerControlActions { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Instance.playerControlActions = new PlayerControls();
            Instance.playerControlActions.Enable();
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Bind(InputAction action, System.Action<InputAction.CallbackContext> callback)
    {
        action.started += callback;
        action.performed += callback;
        action.canceled += callback;
    }
    public void UnBind(InputAction action, System.Action<InputAction.CallbackContext> callback)
    {
        action.started -= callback;
        action.performed -= callback;
        action.canceled -= callback;
    }
}