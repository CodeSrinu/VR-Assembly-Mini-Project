using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class AssemblyManager : MonoBehaviour
{
    private int _currentStep = 0;
    private List<AssemblyStep> _assemblySteps = new List<AssemblyStep>();
    public event Action<AssemblyStep> NewStepGiven;

    private void OnEnable()
    {
        foreach(var step in _assemblySteps)
        {
            step.socket.hoverEntered.AddListener(OnHoverEntered);
            step.socket.hoverExited.AddListener(OnHoverExited);
            step.socket.selectEntered.AddListener(OnSelectEntered);
        }
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        NewStepGiven?.Invoke(_assemblySteps[_currentStep]);
    }
    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        ShowValidationFeedback(args.interactorObject.transform.gameObject);
    }
    private void OnHoverExited(HoverExitEventArgs args)
    {
        //reset the hologram mesh colour
    }

    private void ShowValidationFeedback(GameObject obj)
    {
        if (obj == _assemblySteps[_currentStep].requiredPart)
        {
            //chnage the hologram color to green
        }
        else
        {
            //chnage the hologram color to red
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(args.interactorObject.transform.gameObject == _assemblySteps[_currentStep].requiredPart)
        {

        }
    }
}
