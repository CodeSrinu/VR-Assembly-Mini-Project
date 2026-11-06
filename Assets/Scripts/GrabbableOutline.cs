using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabbableOutline : MonoBehaviour
{
    public Material outlineMat;
    private Material originalMat;
    private XRGrabInteractable xrGrab;
    private MeshRenderer mr;


    private void Awake()
    {
        xrGrab = GetComponent<XRGrabInteractable>();
        mr = GetComponent<MeshRenderer>();
        originalMat = mr.material;
    }

    private void OnEnable()
    {
        xrGrab.hoverEntered.AddListener(OnHoverEntered);
        xrGrab.hoverExited.AddListener(OnHoverExited);
    }

    private void OnDisable()
    {
        xrGrab.hoverEntered.RemoveListener(OnHoverEntered);
        xrGrab.hoverExited.RemoveListener(OnHoverExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if(xrGrab.isSelected) return;
        mr.AddMaterial(outlineMat);
    }
    private void OnHoverExited(HoverExitEventArgs args)
    {
        mr.materials = new Material[] {originalMat};
    }
}
