using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DynamicAttachForSteering : MonoBehaviour
{
    public Transform leftHandAttachTransform;
    public Transform rightHandAttachTransform;

    public InputActionReference leftGripActionRef;
    public InputActionReference rightGripActionRef;
    public XRGrabInteractable xrGrab;

    private void OnEnable()
    {
        leftGripActionRef.action.started += OnGripTriggered;
        rightGripActionRef.action.started += OnGripTriggered;
        leftGripActionRef?.action.Enable();
        rightGripActionRef?.action.Enable();
    }

    private void OnDisable()
    {
        leftGripActionRef.action.started -= OnGripTriggered;
        rightGripActionRef.action.started -= OnGripTriggered;
        leftGripActionRef?.action.Disable();
        rightGripActionRef?.action.Disable();
    }

    private void OnGripTriggered(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device.usages.Contains(CommonUsages.LeftHand))
        {
            xrGrab.attachTransform = leftHandAttachTransform;
        }
        else
        {
            xrGrab.attachTransform= rightHandAttachTransform;
        }
    }
}
