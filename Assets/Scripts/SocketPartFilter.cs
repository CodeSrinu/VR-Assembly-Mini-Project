using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketPartFilter : MonoBehaviour, IXRSelectFilter
{
    public GameObject requiredPart;
    public bool canProcess => isActiveAndEnabled;
    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        var part = interactable.transform.gameObject;

        return part == requiredPart;
    }
}
