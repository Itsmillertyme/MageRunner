using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerHealthUI healthUI;
    [SerializeField] private PlayerHealthUI staminaUI;

    private Coroutine healthRegen;
    private Coroutine staminaRegen;

    public Player PlayerSO => player;

    public static PlayerAbilities Instance { get; private set; } // SINGLETON PATTERN

    private void Awake()
    {
        Instance = this; // SINGLETON PATTERN
    }

    private void Start()
    {
        if (healthRegen == null && player.CurrentHealth < player.MaxHealth)
        {
            healthRegen = StartCoroutine(HealOverTime());
        }

        UpdateHealthUI();
        UpdateStaminaUI();
    }

    public void AddToHealth(int add)
    {
        if (add <= player.MaxHealth - player.CurrentHealth)
        {
            player.Heal(add);
        }
        else
        {
            player.Heal(player.MaxHealth - player.CurrentHealth);
        }

        UpdateHealthUI();
    }

    public void RemoveFromHealth(int remove)
    {
        if (remove < player.CurrentHealth)
        {
            player.TakeDamage(remove);

            if (healthRegen != null)
            {
                StopCoroutine(healthRegen);
            }

            healthRegen = StartCoroutine(HealOverTime());
        }
        else
        {
            player.TakeDamage(player.CurrentHealth);
            Debug.Log("You died");

            //raise player death event
            player.PlayerHasDied.Raise();
        }

        UpdateHealthUI();
    }

    private IEnumerator HealOverTime()
    {
        while (player.CurrentHealth < player.HealthRegenCap)
        {
            yield return new WaitForSeconds(player.HealthRegenFrequency);
            AddToHealth(player.HealthRegenAmount);
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        player.SetMaxHealth(amount);
        UpdateHealthUI();
    }

    private void UpdateHealthUI() {
        float value = (float) player.CurrentHealth / (float) player.MaxHealth;
        if (healthUI != null) {
            healthUI.UpdateImageFill(value);
        }
        else {

            Debug.Log("healthUI is null");
        }

    }

    public void DecreaseStamina(int remove)
    {
        if (remove < player.CurrentStamina)
        {
            player.ReduceCurrentStamina(remove);

            if (staminaRegen != null)
            {
                StopCoroutine(staminaRegen);
            }

            staminaRegen = StartCoroutine(RecoverStaminaOverTime());
        }
        else
        {
            player.ReduceCurrentStamina(player.CurrentStamina);
        }

        UpdateStaminaUI();
    }

    public void IncreaseStamina(int add)
    {
        if (add <= player.MaxStamina - player.CurrentStamina)
        {
            player.IncreaseCurrentStamina(add);
        }
        else
        {
            player.IncreaseCurrentStamina(player.MaxStamina - player.CurrentStamina);
        }

        UpdateStaminaUI();
    }

    private IEnumerator RecoverStaminaOverTime()
    {
        while (player.CurrentStamina < player.MaxStamina)
        {
            yield return new WaitForSeconds(player.StaminaRegenFrequency);
            IncreaseStamina(player.StaminaRegenAmount);
        }
    }

    private void UpdateStaminaUI() {
        float value = (float) player.CurrentStamina / (float) player.MaxStamina;
        if (staminaUI != null) {
            staminaUI.UpdateImageFill(value);
        }
        else {

            Debug.Log("staminaUI is null");
        }
    }
}