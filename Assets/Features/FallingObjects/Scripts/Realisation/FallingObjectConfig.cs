using System;
using UnityEngine;

[Serializable]
public struct PlayerMessageModel
{
    public int DeltaHealth;
    public int DeltaScore;
}

[Serializable]
public struct FallingObjectModel
{
    public Sprite Sprite;    
    public float Speed;
    public PlayerMessageModel PlayerMessageOnClickModel;
    public AudioClip OnClickSound;
    public PlayerMessageModel PlayerMessageEndOfScreenModel;
    public AudioClip OnEoSSound;
}

[CreateAssetMenu(fileName = "FallingObjectConfig", menuName = "Configs/FallingObjectConfig", order = 1)]
public class FallingObjectConfig : ScriptableObject
{
    [SerializeField] private FallingObjectModel[] models;

    public FallingObjectModel[] Models => models;

}
