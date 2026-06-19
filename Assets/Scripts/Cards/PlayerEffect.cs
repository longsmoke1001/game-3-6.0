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
        HealthOnHit,
        reflectedDamageMultiplier,
        CanMoveWhileAttacking,
        CanMoveWhileDefending,
        DefendHealMultiplier,
        ProjectileCount,
        Pierce,
        ProjReturn,
        MovSpeedScaleDamage,
        ProjSpeed,
    }
    [SerializeField] PlayerEffectType effectType;
    [SerializeField] float effectValue;
    //public override string Description => effectType.ToString()+"X"+effectValue;

    public override void Apply()
    {
        Player player = FindAnyObjectByType<Player>();
        switch (effectType)
        {
            case PlayerEffectType.Health:
                player.runtimePlayerData.maxHealth *= effectValue;
                break;
            case PlayerEffectType.AttackPower:
                player.runtimePlayerData.attackPower *= effectValue;
                break;
            case PlayerEffectType.AttackRange:
                player.runtimePlayerData.attackRange *= effectValue;
                break;
            case PlayerEffectType.MovSpeed:
                player.runtimePlayerData.movSpeed *= effectValue;
                break;
            case PlayerEffectType.AttackSpeed:
                player.runtimePlayerData.attackSpeed *= effectValue;
                break;
            case PlayerEffectType.HealthOnHit:
                player.runtimePlayerData.healthOnHit += effectValue;
                break;
            case PlayerEffectType.reflectedDamageMultiplier:
                player.runtimePlayerData.reflectedDamageMultiplier *= effectValue;
                break;
            case PlayerEffectType.CanMoveWhileAttacking:
                player.runtimePlayerData.canMoveWhileAttacking = true;
                break;
            case PlayerEffectType.CanMoveWhileDefending:
                player.runtimePlayerData.canMoveWhileDefending = true;
                break;
            case PlayerEffectType.DefendHealMultiplier:
                player.runtimePlayerData.defendHealMultiplier += effectValue;
                break;
            case PlayerEffectType.ProjectileCount:
                player.runtimePlayerData.projectileCount *= (int)effectValue;
                break;
            case PlayerEffectType.Pierce:
                player.runtimePlayerData.pierce = true;
                break;
            case PlayerEffectType.ProjReturn:
                player.runtimePlayerData.projReturn = true;
                break;
            case PlayerEffectType.MovSpeedScaleDamage:
                player.runtimePlayerData.movSpeedScaleDamage = true;
                break;
            case PlayerEffectType.ProjSpeed:
                player.runtimePlayerData.projSpeed *= 0.5f;
                player.runtimePlayerData.attackPower *= effectValue;
                break;
        }
    }
}
