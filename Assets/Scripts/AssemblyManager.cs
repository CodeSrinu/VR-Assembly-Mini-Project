using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AssemblyManager : MonoBehaviour
{
    private int _currentStep = 0;
    public List<AssemblyStep> assemblySteps = new List<AssemblyStep>();
    public static event Action<AssemblyStep> AssemblyStepGiven;
    public static event Action AssemblyComplete;
    public Hologram hologram;
    public PartsManager partsManager;
    public InstructionsUI instructionsUI;
    public Transform partsparent;
    public Transform sockets;
    public HashSet<GameObject> placedParts = new HashSet<GameObject>();
    private bool isInSectionView = false;
    private AssemblyStep currentStep;
    string[] instructions =
{
    "Position the <color=#55D9FF><b>BODY</b></color> in its designated location",
    "Place the <color=#55D9FF><b>BODY TOP</b></color> onto the main valve body",
    "Turn on the Section View and insert the <color=#55D9FF><b>SEAT</b></color> into the body",
    "Fit the <color=#55D9FF><b>SEAL</b></color> into its designated groove",
    "Position the <color=#55D9FF><b>COLLAR</b></color> onto the assembly",
    "Insert the <color=#55D9FF><b>FILLER</b></color> into its designated position",
    "Position the first <color=#55D9FF><b>SMALL BRIGADE</b></color> component",
    "Insert the first <color=#55D9FF><b>SMALL STUD</b></color> into its mounting hole",
    "Insert the second <color=#55D9FF><b>SMALL STUD</b></color> into its mounting hole",
    "Insert the first <color=#55D9FF><b>NORMAL STUD</b></color> into its mounting hole",
    "Insert the second <color=#55D9FF><b>NORMAL STUD</b></color> into its mounting hole",
    "Insert the third <color=#55D9FF><b>NORMAL STUD</b></color> into its mounting hole",
    "Insert the fourth <color=#55D9FF><b>NORMAL STUD</b></color> into its mounting hole",
    "Insert the fifth <color=#55D9FF><b>NORMAL STUD</b></color> into its mounting hole",
    "Insert the sixth <color=#55D9FF><b>NORMAL STUD</b></color> into its mounting hole",
    "Tighten the first <color=#55D9FF><b>HEX NUT</b></color> onto its corresponding stud",
    "Tighten the second <color=#55D9FF><b>HEX NUT</b></color> onto its corresponding stud",
    "Tighten the third <color=#55D9FF><b>HEX NUT</b></color> onto its corresponding stud",
    "Tighten the fourth <color=#55D9FF><b>HEX NUT</b></color> onto its corresponding stud",
    "Tighten the fifth <color=#55D9FF><b>HEX NUT</b></color> onto its corresponding stud",
    "Tighten the sixth <color=#55D9FF><b>HEX NUT</b></color> onto its corresponding stud",
    "Insert the first <color=#55D9FF><b>BIG STUD</b></color> into its mounting position",
    "Insert the second <color=#55D9FF><b>BIG STUD</b></color> into its mounting position",
    "Position the main <color=#55D9FF><b>BRIGADE</b></color> onto the assembly",
    "Tighten the seventh <color=#55D9FF><b>HEX NUT</b></color> onto its corresponding stud",
    "Tighten the eighth <color=#55D9FF><b>HEX NUT</b></color> onto its corresponding stud",
    "Insert the <color=#55D9FF><b>SPINDLE</b></color> into the valve assembly",
    "Attach the <color=#55D9FF><b>STEERING WHEEL</b></color> to the spindle",
    "Secure the steering wheel using the <color=#55D9FF><b>DOMED CAP NUT</b></color>"
};
    public AssemblyStep[] remainingSteps => assemblySteps.ToArray()[_currentStep..assemblySteps.Count];
    private void OnEnable()
    {
        AssemblyStepGiven += OnNewStepGiven;
        InstructionsUI.AssemblyStarted += OnAssemblyStarted;
        SectionViewUI.SectionViewToggled += (isOn) =>
        {
            isInSectionView = isOn;
            if(!isOn && _currentStep == 6)
            {
                AssemblyStepGiven?.Invoke(assemblySteps[_currentStep]);
            }
        };
    }
    private void OnAssemblyStarted()
    {
        partsManager.ResetAllParts();
        for (int i = 0; i < assemblySteps.Count; i++)
        {
            AssemblyStep step = assemblySteps[i];
            step.socket.socketActive = false;
            step.socket.hoverEntered.AddListener(OnHoverEntered);
            step.socket.hoverExited.AddListener(OnHoverExited);
            step.socket.selectEntered.AddListener(OnSelectEntered);
            step.number = i + 1;

        }
        _currentStep = 0;
        AssemblyStepGiven?.Invoke(assemblySteps[_currentStep]);
    }
    private void OnNewStepGiven(AssemblyStep step)
    {
        Debug.Log($"Current Step: {step.requiredPart.name}, stepNumber: {step.number}");
        step.socket.socketActive = true;

    }
    private void OnDisable()
    {
        foreach (var step in assemblySteps)
        {
            step.socket.hoverEntered.RemoveListener(OnHoverEntered);
            step.socket.hoverExited.RemoveListener(OnHoverExited);
            step.socket.selectEntered.RemoveListener(OnSelectEntered);
        }
    }
    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (_currentStep >= assemblySteps.Count) return;

        if ((IXRInteractor)assemblySteps[_currentStep].socket != args.interactorObject) return;

        if (placedParts.Contains(args.interactableObject.transform.gameObject)) return;

        ShowValidationFeedback(args.interactableObject.transform.gameObject);

    }
    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (_currentStep >= assemblySteps.Count) return;

        if ((IXRInteractor)assemblySteps[_currentStep].socket != args.interactorObject) return;

        hologram?.ResetToHologramMat();
    }
    private void ShowValidationFeedback(GameObject obj)
    {
        if (obj.tag == assemblySteps[_currentStep].requiredPart.tag)
        {
            hologram?.ShowCorrectFeedBack();
        }
        else
        {
            hologram?.ShowIncorrectFeedBack();
        }
    }
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        currentStep = assemblySteps[_currentStep];
        GameObject objPlaced = args.interactableObject.transform.gameObject;
        if (_currentStep >= assemblySteps.Count) return;

        if (objPlaced.tag != currentStep.requiredPart.tag)
            return;

        if ((IXRInteractor)currentStep.socket != args.interactorObject)
            return;
        if (placedParts.Contains(args.interactableObject.transform.gameObject)) return;

        StartCoroutine(WaitForPlacement(objPlaced, currentStep.socket.transform));
        

    }

    private void NextStep()
    {
        _currentStep++;

        if (_currentStep >= assemblySteps.Count)
        {
            AssemblyComplete?.Invoke();
            return;
        }

        if (_currentStep >= 6 && isInSectionView)
        {

            instructionsUI.ShowInstruction("Turn off the section view to continue the assembly");
            hologram?.RemoveCurrentHologram();
            return;
        }

        AssemblyStepGiven?.Invoke(assemblySteps[_currentStep]);
    }

    private IEnumerator WaitForPlacement(GameObject obj, Transform attachTransorm)
    {

        while (Vector3.Distance(obj.transform.position, attachTransorm.position) > 0.005f
            || Quaternion.Angle(obj.transform.rotation, attachTransorm.rotation) > 0.5f)
        {
            yield return null;
        }

        ResetObj(obj);
        placedParts.Add(obj);
        Debug.Log($"Placed part: {obj.name}");
        currentStep.socket.socketActive = false;
        NextStep();
    }
    private void ResetObj(GameObject obj)
    {
        Component[] allComponents = obj.GetComponents<Component>();
        foreach (var comp in allComponents)
        {
            if (comp is XRGrabInteractable || comp is GrabbableOutline || comp is MeshCollider)
            {
                Destroy(comp);
            }
        }

        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Rigidbody>().useGravity = false;
    }
    [ContextMenu("Assign")]
    private void AssignScripts()
    {
        Assign();
        Debug.Log("Dependances are assigned");
    }
    private void Assign()
    {
        assemblySteps.Clear();
        for(int i = 0; i < partsparent.childCount; i++) 
        {
            AssemblyStep step = new AssemblyStep();

            step.requiredPart = partsparent.GetChild(i).gameObject;
            step.socket = sockets.GetChild(i).GetComponent<XRSocketInteractor>();
            step.instruction = instructions[i];
            step.number = i + 1;
            assemblySteps.Add(step);
        }
    }
}
