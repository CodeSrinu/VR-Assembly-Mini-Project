using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerHandsManager : MonoBehaviour
{
    public InputActionReference triggerActionRef;
    public InputActionReference gripActionRef;

    public Animator handAnimator;


    private void Awake()
    {
        handAnimator = GetComponent<Animator>();
        SetupInputActions();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetupInputActions()
    {
        if (triggerActionRef == null || gripActionRef == null) return;

        triggerActionRef.action.performed += OnTriggerPerformed;
        triggerActionRef.action.canceled += OnTriggerCanceled;

        gripActionRef.action.performed += OnGripPerformed;
        gripActionRef.action.canceled += OnGripCanceled;
    }

    private void OnGripCanceled(InputAction.CallbackContext ctx)
    {
        UpdateHandAnimation("Grip", 0);
    }

    private void OnTriggerCanceled(InputAction.CallbackContext ctx)
    {
        UpdateHandAnimation("Trigger", 0);
    }

    private void OnGripPerformed(InputAction.CallbackContext ctx)
    {
        UpdateHandAnimation("Grip", ctx.ReadValue<float>());
    }

    private void OnTriggerPerformed(InputAction.CallbackContext ctx)
    {
        UpdateHandAnimation("Trigger",ctx.ReadValue<float>());
    }

    private void UpdateHandAnimation(string parameterName, float value)
    {
        if (handAnimator == null) return;

        handAnimator.SetFloat(parameterName, value);
    }

    private void OnEnable()
    {
        triggerActionRef?.action.Enable();
        gripActionRef?.action.Enable();
    }

    private void OnDisable()
    {
        triggerActionRef?.action.Disable();
        gripActionRef?.action.Disable();
    }
}
