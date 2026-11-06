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
            partsManager.ResetParts(assemblyManager.remainingSteps);
        });
    }
}
