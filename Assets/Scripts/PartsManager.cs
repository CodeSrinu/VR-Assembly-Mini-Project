using System.Collections.Generic;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    public Transform partsParent;
    //public Transform socketsParent;
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

    public void ResetParts(AssemblyStep[] steps)
    {
        foreach (var step in steps)
        {
            step.requiredPart.transform.localPosition = partsInitialPositions[step.requiredPart];
            step.requiredPart.transform.localRotation = partsInitialRotations[step.requiredPart];
        }
    }

    private void SetPosAndRotForAssembly(Transform partsParent, Transform socketsParent)
    {
        for (int i = 0; i < partsParent.childCount; i++)
        {
            Transform part = partsParent.GetChild(i);
            Transform socket = socketsParent.GetChild(i);

            part.position = socket.position;
            part.rotation = socket.rotation;
        }
        
    }

    //[ContextMenu("Align Parts To Sockets")]
    //public void AlignPartsInEditor()
    //{
    //    SetPosAndRotForAssembly(partsParent, socketsParent);
    //}
}
