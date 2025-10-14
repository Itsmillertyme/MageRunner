using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Base Stats")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int xpGrantedOnDeath;

    [Header("References")]
    private XPSystem levelingSystem;
    [SerializeField] private EnemyHealthUI ui;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        levelingSystem = FindFirstObjectByType<XPSystem>();
    }

    public void RemoveFromHealth(int amountToRemove)
    {
        if (amountToRemove < currentHealth)
        {
            currentHealth -= amountToRemove;
            UpdateUI();
        }
        else
        {
            currentHealth = 0;
            UpdateUI();
            // call new death script here and move following logic there
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        levelingSystem.AddXP(xpGrantedOnDeath);
    }

    private void UpdateUI()
    {
        float value = (float)currentHealth / (float)maxHealth;
        ui.UpdateHealthBar(value);
    }
}