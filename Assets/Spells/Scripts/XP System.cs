using UnityEngine;

public class XPSystem : MonoBehaviour
{
    private int spellIndex = 0;
    [SerializeField] private SkillTree[] skillTrees;
    [SerializeField] private Spell[] spells;

    private SpellBook spellBook;

    private void Awake()
    {
        // GET REFERENCES THEN DISABLE UI
        skillTrees = GetComponentsInChildren<SkillTree>();
        spellBook = FindFirstObjectByType<SpellBook>();

        foreach (SkillTree skillTree in skillTrees)
        {
            skillTree.gameObject.SetActive(false);
        }

        // FILL SPELL ARRAY WITH SPELLS
        spells = new Spell[skillTrees.Length];

        for (int i = 0; i < skillTrees.Length; i++)
        {
            spells[i] = (Spell)skillTrees[i].AbilityToUpgrade;
        }
    }

    public void AddXP(int xpGained)
    {
        // IF MAX LEVEL, RETURN
        if (spells[spellIndex].CurrentLevel == spells[spellIndex].MaxLevel) return;

        spells[spellIndex].AddToXP(xpGained);

        // IF LEVEL REQUIREMENTS ARE MET, LEVEL UP
        if (spells[spellIndex].CurrentXP >= spells[spellIndex].XPToLevelUp)
        {
            LevelUp();
            spellBook.UpdateUI();
        }
    }

    private void LevelUp()
    {
        // LEVEL UP, UPDATE XP TO NEXT LEVEL REQUIREMENT, & TELL SKILL TREE TO ADD A SKILL POINT
        spells[spellIndex].LeveledUp();
        spells[spellIndex].SetNextLevelUpRequirements();
        skillTrees[spellIndex].SkillPointEarned();
        spellBook.UpdateUI();
    }

    public void SetIndex(int index) => spellIndex = index;

}