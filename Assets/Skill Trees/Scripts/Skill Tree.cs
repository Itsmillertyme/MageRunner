using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private Spell spell;
    [SerializeField] private int skillPoints;
    private SkillTreeUIController uiController;
    private readonly HashSet<SkillNode> ownedUpgrades = new();
    public int SkillPoints => skillPoints;
    public Spell SelectedSpell => spell;

    private void Awake()
    {
        uiController = GetComponentInParent<SkillTreeUIController>();
    }

    // CHECK IF UPGRADE IS ALREADY OWNED
    public bool UpgradeOwned(SkillNode upgrade)
    {
        return ownedUpgrades.Contains(upgrade);
    }   

    // CHECK IF ELIGIBLE TO UPGRADE
    public DoubleBool CanUpgrade(SkillNode upgrade)
    {
        return upgrade.CanUpgrade(ownedUpgrades, skillPoints);
    }

    // PERFORM THE UPGRADE
    public void ApplyUpgrade(SkillNode upgrade)
    {
        // IF SHOULD NOT UPGRADE, RETURN
        if (UpgradeOwned(upgrade) || !CanUpgrade(upgrade).MeetsAllRequirements())
        {
            return;
        }

        // UPGRADE
        ownedUpgrades.Add(upgrade);
        upgrade.ApplyUpgrade(spell);
        skillPoints -= upgrade.UpgradeCost;

        // UPDATE BUTTON TEXT AND INTERACTABILITY
        foreach (var button in FindObjectsByType<SkillUpgradeButton>(FindObjectsSortMode.None))
        {
            button.UpdateButtonState();
        }

        uiController.UpdateSkillPoints();
    }

    public void SkillPointEarned()
    {
        skillPoints++;

        // UPDATE BUTTON TEXT AND INTERACTABILITY
        foreach (var button in FindObjectsByType<SkillUpgradeButton>(FindObjectsSortMode.None))
        {
            button.UpdateButtonState();
        }
    }
}