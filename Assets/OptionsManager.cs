using UnityEngine;
using MixedReality.Toolkit.UX;

public class OptionsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        musicSlider.OnValueUpdated.AddListener(UpdateMusicSlider);
        sfxSlider.OnValueUpdated.AddListener(UpdateSFXSlider);
    }

    void UpdateMusicSlider(SliderEventData eventData)
    {
        AudioManager.Instance.musicSource.volume = eventData.NewValue;
    }

    void UpdateSFXSlider(SliderEventData eventData)
    {
        AudioManager.Instance.sfxSource.volume = eventData.NewValue;
    }
}
