using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioPauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject mixerCanvas;
    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider playerSlider;
    [SerializeField] private TextMeshProUGUI ambienceValueText;
    [SerializeField] private TextMeshProUGUI musicValueText;
    [SerializeField] private TextMeshProUGUI playerValueText;
    [SerializeField] private Button applyButton;

    [Header("Audio References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string ambienceParam = "AmbienceVolume";
    [SerializeField] private string musicParam = "MusicVolume";
    [SerializeField] private string playerParam = "PlayerVolume";

    private float ambienceVolume = 1f;
    private float musicVolume = 1f;
    private float playerVolume = 1f;

    private const float MIN_DB = -80f;

    public bool isPaused { get; private set; } = false;

    private void Start()
    {
        mixerCanvas.SetActive(false);

        ambienceSlider.onValueChanged.AddListener(OnAmbienceSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        playerSlider.onValueChanged.AddListener(OnPlayerSliderChanged);

        if (applyButton != null)
            applyButton.onClick.AddListener(ApplyChanges);

        if (audioMixer != null)
        {
            audioMixer.GetFloat(ambienceParam, out float ambienceDB);
            audioMixer.GetFloat(musicParam, out float musicDB);
            audioMixer.GetFloat(playerParam, out float playerDB);

            ambienceVolume = DbToNormalized(ambienceDB);
            musicVolume = DbToNormalized(musicDB);
            playerVolume = DbToNormalized(playerDB);

            UpdateSliders();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMixerCanvas();
        }
    }

    private void ToggleMixerCanvas()
    {
        bool newState = !mixerCanvas.activeSelf;
        isPaused = newState;
        mixerCanvas.SetActive(newState);

        if (newState)
        {
            UpdateSliders();
        }
    }

    private void UpdateSliders()
    {
        ambienceSlider.value = ambienceVolume;
        musicSlider.value = musicVolume;
        playerSlider.value = playerVolume;

        UpdateValueTexts();
    }

    private void UpdateValueTexts()
    {
        if (ambienceValueText != null)
            ambienceValueText.text = "Ambience: " + Mathf.RoundToInt(ambienceVolume * 100) + "%";

        if (musicValueText != null)
            musicValueText.text = "Music: " + Mathf.RoundToInt(musicVolume * 100) + "%";

        if (playerValueText != null)
            playerValueText.text = "Player: " + Mathf.RoundToInt(playerVolume * 100) + "%";
    }

    private void OnAmbienceSliderChanged(float value)
    {
        ambienceVolume = value;
        UpdateValueTexts();

        if (audioMixer != null)
            audioMixer.SetFloat(ambienceParam, NormalizedToDb(value));
    }

    private void OnMusicSliderChanged(float value)
    {
        musicVolume = value;
        UpdateValueTexts();

        if (audioMixer != null)
            audioMixer.SetFloat(musicParam, NormalizedToDb(value));
    }

    private void OnPlayerSliderChanged(float value)
    {
        playerVolume = value;
        UpdateValueTexts();

        if (audioMixer != null)
            audioMixer.SetFloat(playerParam, NormalizedToDb(value));
    }

    private void ApplyChanges()
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(ambienceParam, NormalizedToDb(ambienceVolume));
            audioMixer.SetFloat(musicParam, NormalizedToDb(musicVolume));
            audioMixer.SetFloat(playerParam, NormalizedToDb(playerVolume));

            PlayerPrefs.SetFloat("AudioMixer_Ambience", ambienceVolume);
            PlayerPrefs.SetFloat("AudioMixer_Music", musicVolume);
            PlayerPrefs.SetFloat("AudioMixer_Player", playerVolume);
            PlayerPrefs.Save();
        }

        mixerCanvas.SetActive(false);
        isPaused = false;
    }

    private float DbToNormalized(float dB)
    {
        return Mathf.Clamp01(1f - (dB / MIN_DB));
    }

    private float NormalizedToDb(float normalized)
    {
        normalized = Mathf.Clamp01(normalized);
        if (normalized == 0)
            return MIN_DB;

        return Mathf.Log10(normalized) * 20f;
    }
}
