using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSources;

    public void SetAudio(AudioSource _audioSources)
    {
        audioSources = _audioSources;
    }

    public void ChangedSlider(Slider slider)
    {
        slider.onValueChanged.AddListener(delegate { ChangedAudioVolume(slider, audioSources); });
    }
    
    private void ChangedAudioVolume(Slider slider, AudioSource audio)
    {
        audioSources.volume = slider.value;
    }
}
