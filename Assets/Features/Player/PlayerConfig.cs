using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig", order = 0)]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private int startHealth;

    public int StartHealth => startHealth;
}
