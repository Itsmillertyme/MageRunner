using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [Header("Health Base Stats")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int xpGrantedOnDeath;

    [Header("References")]
    private XPSystem levelingSystem;
    private LootSystem lootSystem;
    [SerializeField] private EnemyHealthUI ui;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake() {
        levelingSystem = FindFirstObjectByType<XPSystem>();
        lootSystem = FindFirstObjectByType<LootSystem>();
    }

    public void RemoveFromHealth(int amountToRemove) {
        if (amountToRemove < currentHealth) {
            currentHealth -= amountToRemove;
            UpdateUI();
        }
        else {
            currentHealth = 0;
            UpdateUI();
            // call new death script here and move following logic there
            levelingSystem.AddXP(xpGrantedOnDeath);
            lootSystem.SelectThenDropLoot(transform.position);
            EnemyDie enemyDie = GetComponent<EnemyDie>();
            enemyDie.Die();
        }
    }

    private void UpdateUI() {
        float value = (float) currentHealth / (float) maxHealth;
        ui.UpdateHealthBar(value);
    }
}