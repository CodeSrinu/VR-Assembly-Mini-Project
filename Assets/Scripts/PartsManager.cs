using System.Collections.Generic;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    public Transform partsParent;
    private Dictionary<GameObject, Vector3> partsInitialPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> partsInitialRotations = new Dictionary<GameObject, Quaternion>();

    private void OnEnable()
    {
        foreach (Transform part in partsParent)
        {
            partsInitialPositions[part.gameObject] = part.localPosition;
            partsInitialRotations[part.gameObject] = part.localRotation;
        }
    }

    public void ResetAllParts()
    {
        foreach (Transform part in partsParent)
        {
            part.localPosition = partsInitialPositions[part.gameObject];
            part.localRotation = partsInitialRotations[part.gameObject];
        }
    }

    public void ResetParts(AssemblyStep[] parts)
    {
        foreach (var part in parts)
        {
            part.requiredPart.transform.localPosition = partsInitialPositions[part.requiredPart];
            part.requiredPart.transform.localRotation = partsInitialRotations[part.requiredPart];
        }
    }
}
