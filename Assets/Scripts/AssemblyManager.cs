using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AssemblyManager : MonoBehaviour
{
    private int _currentStep = 0;
    public List<AssemblyStep> assemblySteps = new List<AssemblyStep>();
    public static event Action<AssemblyStep> NewStepGiven;
    public Hologram hologram;

    private void OnEnable()
    {
        foreach(var step in assemblySteps)
        {
            step.socket.hoverEntered.AddListener(OnHoverEntered);
            step.socket.hoverExited.AddListener(OnHoverExited);
            step.socket.selectEntered.AddListener(OnSelectEntered);
        }
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

    private void Start()
    {
        NewStepGiven?.Invoke(assemblySteps[_currentStep]);
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
        if ((objPlaced == assemblySteps[_currentStep].requiredPart) && (IXRInteractor)assemblySteps[_currentStep].socket == args.interactorObject)
        {
            args.interactableObject.registered += (InteractableRegisteredEventArgs) => ResetObj(objPlaced);
            _currentStep++;

            if (_currentStep <= assemblySteps.Count)
                NewStepGiven?.Invoke(assemblySteps[_currentStep]);
            else
                Debug.Log("Asembling Complete");
        }
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
