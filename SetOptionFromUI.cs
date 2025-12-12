using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetOptionFromUI : MonoBehaviour
{
    public Slider bgmSlider;  
    public Slider sfxSlider;
    public TMPro.TMP_Dropdown turnDropdown;
    public SetTurnTypeFromPlayerPref turnTypeFromPlayerPref;
    public AudioMixer mixer; 

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        turnDropdown.onValueChanged.AddListener(SetTurnPlayerPref);

  
        if (PlayerPrefs.HasKey("turn"))
            turnDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("turn"));

        float bgmVolume;
        if (mixer.GetFloat("BGMVolume", out bgmVolume))
            bgmSlider.SetValueWithoutNotify(Mathf.Pow(10, bgmVolume / 20f));

        float sfxVolume;
        if (mixer.GetFloat("SFXVolume", out sfxVolume))
            sfxSlider.SetValueWithoutNotify(Mathf.Pow(10, sfxVolume / 20f));
    }

    public void SetBGMVolume(float value)
    {
        mixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
    }

    public void SetTurnPlayerPref(int value)
    {
        PlayerPrefs.SetInt("turn", value);
        turnTypeFromPlayerPref.ApplyPlayerPref();
    }
}
