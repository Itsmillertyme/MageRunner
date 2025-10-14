using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour {

    [Header("Player References")]
    [SerializeField] private PlayerAttributes player;
    [SerializeField] private PlayerHealthUI ui;

    [Header("Miscellaneous References")]
    [SerializeField] GameEvent playerDeathEvent;

    private Coroutine healthRegen;

    private void Start() {
        if (healthRegen == null && player.CurrentHealth < player.MaxHealth) {
            healthRegen = StartCoroutine(HealOverTime());
        }

        UpdateUI();
    }

    public void AddToHealth(int add) {
        if (add <= player.MaxHealth - player.CurrentHealth) {
            player.SetCurrentHealth(add);
        }
        else {
            player.SetCurrentHealth(player.MaxHealth - player.CurrentHealth);
        }

        UpdateUI();
    }

    public void RemoveFromHealth(int remove) {
        if (remove < player.CurrentHealth) {
            player.SetCurrentHealth(-remove);

            if (healthRegen != null) {
                StopCoroutine(healthRegen);
            }

            healthRegen = StartCoroutine(HealOverTime());
        }
        else {
            player.SetCurrentHealth(-player.CurrentHealth);
            //Destroy(this);
            //Time.timeScale = 0f;
            Debug.Log("You died");

            //raise player death event
            playerDeathEvent.Raise();
        }

        UpdateUI();
    }

    public bool HealthIsFull() {
        bool healthIsFull = true;

        if (player.CurrentHealth < player.MaxHealth) {
            return !healthIsFull;
        }

        return healthIsFull;
    }

    private IEnumerator HealOverTime() {
        while (player.CurrentHealth < player.MaxHealth) {
            yield return new WaitForSeconds(player.HealthRegenFrequency);
            AddToHealth(player.HealthRegenAmount);
        }
    }

    public void IncreaseMaxHealth(int amount) {
        player.SetMaxHealth(amount);
        UpdateUI();
    }

    private void UpdateUI() {
        float value = (float) player.CurrentHealth / (float) player.MaxHealth;
        ui.UpdateImageFill(value);
    }
}