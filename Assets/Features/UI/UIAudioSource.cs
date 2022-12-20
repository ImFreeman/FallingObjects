using UnityEngine;
using Zenject;

public class UIAudioSource
{
    public bool IsLooped
    {
        get => audioSource.loop;
        set => audioSource.loop = value;
    }

    private AudioSource audioSource;
    public UIAudioSource(IInstantiator instantiator)
    {
        audioSource = instantiator.CreateEmptyGameObject("audioUI").AddComponent(typeof(AudioSource)) as AudioSource;
    }

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void Stop()
    {
        audioSource.Stop();
    }
}
