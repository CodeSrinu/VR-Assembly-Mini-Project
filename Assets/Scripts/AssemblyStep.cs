using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[System.Serializable]
public struct AssemblyStep
{
    public GameObject requiredPart;
    public XRSocketInteractor socket;
    public string instruction;
}
