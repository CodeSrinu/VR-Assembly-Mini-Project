using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectionViewUI : MonoBehaviour
{
    public Button btn;
    private bool isInSectionView = false;
    public static event Action<bool> SectionViewToggled;

    private void Start()
    {
        btn.onClick.AddListener(OnBtnClick);
        AssemblyManager.AssemblyStepGiven += OnNewStepGiven;
        gameObject.SetActive(false);
    }

    private void OnNewStepGiven(AssemblyStep step)
    {
        if(step.number == 3f)
            gameObject.SetActive(true);
    }

    private void OnBtnClick()
    {
        AudioManager.Instance.PlayButtonClick();
        isInSectionView = !isInSectionView;
        SectionViewToggled?.Invoke(isInSectionView);

        btn.GetComponentInChildren<TextMeshProUGUI>().text = isInSectionView ? "Off" : "On";
    }
}
