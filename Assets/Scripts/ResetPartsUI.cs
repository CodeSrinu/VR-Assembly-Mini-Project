using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetPartsUI : MonoBehaviour
{
    public Button btn;
    public PartsManager partsManager;
    public AssemblyManager assemblyManager;

    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            List<AssemblyStep> unAssembledParts = new List<AssemblyStep>();
            foreach (AssemblyStep step in assemblyManager.assemblySteps)
            {
                if (!assemblyManager.placedParts.Contains(step.requiredPart))
                {
                    unAssembledParts.Add(step);
                }
            }

            partsManager.ResetParts(unAssembledParts.ToArray());
        });
    }
}
