using UnityEngine;
using UnityEngine.InputSystem;

public class ControlInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionsAsset;

    private void Start()
    {
        SettingsManager.Instance.onInputChanged += UpdateInputType;

        UpdateInputType(SettingsManager.Instance.settings.inputType);
    }
    private void OnDestroy()
    {
        SettingsManager.Instance.onInputChanged -= UpdateInputType;
    }
    public void UpdateInputType(InputType selectedInput)
    {
        InputActionMap swipeMap = inputActionsAsset.FindActionMap("Touch", true);

        if (selectedInput == InputType.Swipe)
        {
            swipeMap.Enable();
        } else
        {
            swipeMap.Disable();
        }
    }
}
