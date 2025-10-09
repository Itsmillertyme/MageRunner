using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Spell Skill Tree/Upgrade Node")]

public class SkillNode : ScriptableObject
{
    [Tooltip("This name will show on the button's text")]
    [SerializeField] private string upgradeName; // UPGRADE BUTTON TEXT NAME
    [SerializeField] private int upgradeCost; // AMOUNT OF SKILL POINTS REQUIRED TO UPGRADE
    [SerializeField] private SkillNode[] prerequisiteUpgrades; // UPGRADES REQUIRED BEFORE THIS NODE
    [SerializeField] private SkillUpgrade upgrade; // BOOST TO APPLY IN UPGRADE

    // GETTERS
    public string UpgradeName => upgradeName;
    public int UpgradeCost => upgradeCost;

    public DoubleBool CanUpgrade(HashSet<SkillNode> ownedUpgrades, int availableSkillPoints)
    {
        bool hasPrereqs = true;
        bool hasSkillPoints = true;

        foreach (SkillNode prerequisite in prerequisiteUpgrades)
        {
            if (!ownedUpgrades.Contains(prerequisite))
            {
                hasPrereqs = false;
            }
        }

        if (upgradeCost > availableSkillPoints)
        {
            hasSkillPoints = false;
        }

        return new DoubleBool(hasPrereqs, hasSkillPoints);
    }

    public void ApplyUpgrade(Spell spell)
    {
        upgrade.Apply(spell);
    }
}

public struct DoubleBool
{
    private bool hasPrereqs;
    private bool hasSkillPoints;

    public DoubleBool(bool hasPrereqs, bool hasSkillPoints)
    {
        this.hasPrereqs = hasPrereqs;
        this.hasSkillPoints = hasSkillPoints;
    }

    public bool HasPrereqsButNotSkillPoints() => hasPrereqs && !hasSkillPoints;
    public bool HasSkillPointsButNotPrereqs() => !hasPrereqs && hasSkillPoints;
    public bool MeetsAllRequirements() => hasPrereqs && hasSkillPoints;
}