using UnityEngine;
[CreateAssetMenu(fileName = "FallingObjectsFallingConfig", menuName = "Configs/FallingObjectsFallingConfig", order = 1)]
public class FallingObjectsFallingConfig : ScriptableObject
{
    [SerializeField] private float spawnDelay;

    public float SpawnDelay { get => spawnDelay; }    
}
