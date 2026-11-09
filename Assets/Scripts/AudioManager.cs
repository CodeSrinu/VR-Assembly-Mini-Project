using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip buttonClick;
    public AudioClip correctHover;
    public AudioClip incorrectHover;
    public AudioClip partPlacement;
    public AudioClip assemblyComplete;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Play(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void PlayButtonClick() => Play(buttonClick);
    public void PlayCorrectHover() => Play(correctHover);
    public void PlayIncorrectHover() => Play(incorrectHover);
    public void PlayPartPlacement() => Play(partPlacement);
    public void PlayAssemblyComplete() => Play(assemblyComplete);
}

