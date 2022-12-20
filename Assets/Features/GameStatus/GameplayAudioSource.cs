using UnityEngine;
using Zenject;

public class GameplayAudioSource
{
    public bool IsLooped
    {
        get => audioSource.loop;
        set => audioSource.loop = value;
    }

    private AudioSource audioSource;
    public GameplayAudioSource(IInstantiator instantiator)
    {
        audioSource = instantiator.CreateEmptyGameObject("audioGameplay").AddComponent(typeof(AudioSource)) as AudioSource;
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
