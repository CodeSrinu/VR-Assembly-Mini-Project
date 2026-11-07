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

        bool isCorrect = part == requiredPart;

        Debug.Log(
            $"Socket: {name} | Incoming: {part.name} | Expected: {requiredPart.name} | Allowed: {isCorrect}"
        );

        return isCorrect;
    }
}
