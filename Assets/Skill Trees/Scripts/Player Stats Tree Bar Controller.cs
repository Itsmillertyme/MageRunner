using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsTreeBarController : MonoBehaviour {
    [Header("Fill Bar Images")]
    [SerializeField] Image HealthBar;
    [SerializeField] Image StrengthBar;
    [SerializeField] Image StaminaBar;
    [SerializeField] Image EnduranceBar;
    [SerializeField] Image HealingBar;
    [SerializeField] Image ManaBar;
    [SerializeField] Image MagicBar;
    [SerializeField] Image LuckBar;

    [Header("Stat References")]
    [SerializeField] Player playerStats;
    [SerializeField] Player defaultStats;


    private void Awake() {
        UpdateStatBars();
    }

    public void UpdateStatBars() {
        //Health
        HealthBar.fillAmount = (float) playerStats.MaxHealth / (2 * defaultStats.MaxHealth);
        HealthBar.GetComponentInChildren<TextMeshProUGUI>().text = $"{playerStats.MaxHealth}/{2 * defaultStats.MaxHealth}";
        //Strength
        StrengthBar.fillAmount = (float) playerStats.MeleeDamage / (2 * defaultStats.MeleeDamage);
        StrengthBar.GetComponentInChildren<TextMeshProUGUI>().text = $"{playerStats.MeleeDamage}/{2 * defaultStats.MeleeDamage}";
        //Stamina
        StaminaBar.fillAmount = (float) playerStats.MaxStamina / (2 * defaultStats.MaxStamina);
        StaminaBar.GetComponentInChildren<TextMeshProUGUI>().text = $"{playerStats.MaxStamina}/{2 * defaultStats.MaxStamina}";
        //Endurance
        EnduranceBar.fillAmount = (float) playerStats.DamageResistance / (2 * defaultStats.DamageResistance);
        EnduranceBar.GetComponentInChildren<TextMeshProUGUI>().text = $"{playerStats.DamageResistance}/{2 * defaultStats.DamageResistance}";
        //Healing
        HealingBar.fillAmount = (float) playerStats.HealingEfficiency / (2 * defaultStats.HealingEfficiency);
        HealingBar.GetComponentInChildren<TextMeshProUGUI>().text = $"{playerStats.HealingEfficiency}/{2 * defaultStats.HealingEfficiency}";
        //Mana
        ManaBar.fillAmount = 0;
        ManaBar.GetComponentInChildren<TextMeshProUGUI>().text = $"{0}/{0}";
        //Magic
        MagicBar.fillAmount = 0;
        MagicBar.GetComponentInChildren<TextMeshProUGUI>().text = $"{0}/{0}";
        //Luck
        LuckBar.fillAmount = (float) playerStats.LootDropLuck / (2 * defaultStats.LootDropLuck);
        LuckBar.GetComponentInChildren<TextMeshProUGUI>().text = $"{playerStats.LootDropLuck}/{2 * defaultStats.LootDropLuck}";
    }
}
