using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "AI/Enemy Profile")]
public class EnemyProfile : ScriptableObject {
    [Header("Aggro & Leash")]
    [Tooltip("Radius in which this enemy detects and begins chasing the player.")]
    public float aggroRadius = 12f;
    [Tooltip("If the player moves outside this range, the enemy returns to its guard or patrol position.")]
    public float leashRadius = 20f;

    [Header("Attack Distance Bands")]
    [Tooltip("Minimum distance before the enemy backs off.")]
    public float attackMinRange = 1.5f;
    [Tooltip("Ideal combat distance for melee or range attacks.")]
    public float attackIdealRange = 2.5f;
    [Tooltip("Maximum range before the enemy resumes chasing.")]
    public float attackMaxRange = 3.5f;

    [Header("Movement Settings")]
    public float moveSpeed = 3.5f;
    public float angularSpeed = 360f;
    public bool use2DFlipFacing = true;

    [Header("Combat Settings")]
    public AttackType attackType = AttackType.Melee;
    public float baseAttackCooldown = 1.5f;
    public float attackRadius = 1.5f;
    public int damage = 10;

    [Header("Melee Settings")]
    [Tooltip("Delay before the hit is registered after attack begins.")]
    public float windUpTime = 0.35f;
    [Tooltip("Delay after hit before next action can occur.")]
    public float recoveryTime = 0.5f;

    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 8f;

    [Header("Debug")]
    public bool showCombatGizmos = false;
    public bool showMovementGizmos = false;
}

public enum AttackType {
    Melee,
    Ranged,
    Super
}
