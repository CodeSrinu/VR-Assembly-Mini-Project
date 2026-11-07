using System;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsUI : MonoBehaviour
{
    public TextMeshProUGUI instructionTxtComp;
    public Button startBtn;
    public static event Action AssemblyStarted;
    public float instructionsFontSize = 10f;

    private void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            AssemblyStarted?.Invoke();
            startBtn.gameObject.SetActive(false);
            instructionTxtComp.autoSizeTextContainer = false;
            instructionTxtComp.fontSize = instructionsFontSize;
        });

        AssemblyManager.AssemblyComplete += OnAssemblyComplete;
    }

    private void OnAssemblyComplete()
    {
        ShowInstruction("Congratualations!, you successfully Assembled the Stop Valve");
        startBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
        startBtn.onClick.RemoveAllListeners();
        startBtn.onClick.AddListener(() => AssemblyStarted?.Invoke());
        startBtn.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        AssemblyManager.AssemblyStepGiven += (step) => ShowInstruction(step.instruction);
    }

    private void ShowInstruction(string instructionTxt)
    {
        instructionTxtComp.text = instructionTxt;
    }

}
