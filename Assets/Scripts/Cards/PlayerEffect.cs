using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEffect", menuName = "CardEffects/PlayerEffect")]
public class PlayerEffect : Card
{
    // Start is called before the first frame update
    public enum PlayerEffectType
    {
        Health,
        AttackPower,
        AttackRange,
        MovSpeed,
        AttackSpeed,
        HealthOnHit
    }
    [SerializeField] PlayerEffectType effectType;
    [SerializeField] float effectValue;
    [SerializeField] Player player;
    //public override string Description => effectType.ToString()+"X"+effectValue;

    public override void Apply()
    {
        switch(effectType)
        {
           case PlayerEffectType.Health:
                player.playerData.maxHealth *= effectValue;
                break;
            case PlayerEffectType.AttackPower:
                player.playerData.attackPower *= effectValue;
                break;
            case PlayerEffectType.AttackRange:
                player.playerData.attackRange *= effectValue;
                break;
            case PlayerEffectType.MovSpeed:
                player.playerData.movSpeed *= effectValue;
                break;
            case PlayerEffectType.AttackSpeed:
                player.playerData.attackSpeed *= effectValue;
                break;
            case PlayerEffectType.HealthOnHit:
                player.playerData.healthOnHit += effectValue;
                break;
        }
    }
}
