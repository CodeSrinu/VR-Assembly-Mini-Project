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
    
    public AssemblyStep[] remainingSteps => assemblySteps.ToArray()[_currentStep .. assemblySteps.Count];
    private void OnEnable()
    {
        AssemblyStepGiven += OnNewStepGiven;
        InstructionsUI.AssemblyStarted += OnAssemblyStarted;
    }

    private void OnAssemblyStarted()
    {
        partsManager.ResetAllParts();
        foreach (var step in assemblySteps)
        {
            step.socket.socketActive = false;
            step.socket.hoverEntered.AddListener(OnHoverEntered);
            step.socket.hoverExited.AddListener(OnHoverExited);
            step.socket.selectEntered.AddListener(OnSelectEntered);
        }
        _currentStep = 0;
        AssemblyStepGiven?.Invoke(assemblySteps[_currentStep]);
    }

    private void OnNewStepGiven(AssemblyStep step)
    {
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
        if((IXRInteractor)assemblySteps[_currentStep].socket == args.interactorObject)
        {
            ShowValidationFeedback(args.interactableObject.transform.gameObject);
        }
    }
    private void OnHoverExited(HoverExitEventArgs args)
    {
        if((IXRInteractor)assemblySteps[_currentStep].socket == args.interactorObject)
        {
            hologram.ResetToHologramMat();
        }
    }

    private void ShowValidationFeedback(GameObject obj)
    {
        if (obj == assemblySteps[_currentStep].requiredPart)
        {
            hologram.ShowCorrectFeedBack();
        }
        else
        {
            hologram.ShowIncorrectFeedBack();
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        GameObject objPlaced = args.interactableObject.transform.gameObject;
        if (objPlaced != assemblySteps[_currentStep].requiredPart)
            return;

        if (assemblySteps[_currentStep].socket.tag != args.interactorObject.transform.gameObject.tag)
            return;

        StartCoroutine(WaitForPlacement(objPlaced, assemblySteps[_currentStep].socket.transform));
        _currentStep++;

        if (_currentStep <= assemblySteps.Count)
            AssemblyStepGiven?.Invoke(assemblySteps[_currentStep]);
        else
            AssemblyComplete?.Invoke();

    }

    private IEnumerator WaitForPlacement(GameObject obj, Transform attachTransorm)
    {
        
        while(Vector3.Distance(obj.transform.position, attachTransorm.position) > 0.001f
            || Quaternion.Angle(obj.transform.rotation, attachTransorm.rotation) > 0.1f)
        {
            yield return null;
        }

        ResetObj(obj);
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
}
