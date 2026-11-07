using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PartHoverUI : MonoBehaviour
{
    public TextMeshProUGUI partNameText;
    public GameObject background;
    public float yOffset;
    public float zOffset;

    public XRGrabInteractable[] parts;

    private void Start()
    {
        background.SetActive(false);

        foreach (var part in parts)
        {
            part.hoverEntered.AddListener(OnHoverEntered);
            part.hoverExited.AddListener(OnHoverExited);
            part.selectEntered.AddListener(OnSelectEntered);
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        GameObject hoveredPart = args.interactableObject.transform.gameObject;

        partNameText.text = hoveredPart.tag;

        Vector3 pos = hoveredPart.transform.position
                    + Vector3.up * yOffset
                    + Vector3.back * zOffset;

        transform.position = pos;
        background.SetActive(true);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        background.SetActive(false);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        background.SetActive(false);
    }
}