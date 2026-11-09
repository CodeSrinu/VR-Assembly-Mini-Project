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
            AudioManager.Instance.PlayButtonClick();
            AssemblyStarted?.Invoke();
            startBtn.gameObject.SetActive(false);
            instructionTxtComp.autoSizeTextContainer = false;
            instructionTxtComp.fontSize = instructionsFontSize;
        });

        AssemblyManager.AssemblyComplete += OnAssemblyComplete;
    }

    private void OnAssemblyComplete()
    {
        AudioManager.Instance.PlayAssemblyComplete();
        ShowInstruction("Congratualations!, you successfully Assembled the Stop Valve");
        startBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
        startBtn.onClick.RemoveAllListeners();
        startBtn.onClick.AddListener(() => {
            AudioManager.Instance.PlayButtonClick();
            SceneManager.LoadScene(0);
        });
        startBtn.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        AssemblyManager.AssemblyStepGiven += (step) => ShowInstruction(step.instruction);
    }

    public void ShowInstruction(string instructionTxt)
    {
        instructionTxtComp.text = instructionTxt;
    }

}
