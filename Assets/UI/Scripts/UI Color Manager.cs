using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/UI Colors")]

public class Colors : ScriptableObject
{
    [Header("Item UI colors")]
    [SerializeField] Color legendaryColor = new (255, 148, 0, 255); // IDK WHY BUT THESE 4 VALUES ARE NOT BEING ACCURATELY PLUGGED IN. USING SERIALIZEFIELD OPTION TO ENSURE ACCURACY. THIS IS JUST A BACKUP OF THOSE VALUES.
    [SerializeField] Color exoticColor = new (207, 0, 255, 255);
    [SerializeField] Color rareColor = new(0, 253, 255, 255);
    [SerializeField] Color uncommonColor = new(55, 255, 0, 255);
    [SerializeField] Color commonColor = new(255, 255, 255, 255);

    [Header("Getters")]
    public Color LegendaryColor => legendaryColor;
    public Color ExoticColor => exoticColor;
    public Color RareColor => rareColor;
    public Color UncommonColor => uncommonColor;
    public Color CommonColor => commonColor;

    public Color GetItemColor(Rarity rarity)
    {
        Color color = Color.black;
        switch (rarity)
        {
            case Rarity.Legendary:
                color = legendaryColor;
                break;
            case Rarity.Exotic:
                color = exoticColor;
                break;
            case Rarity.Rare:
                color = rareColor;
                break;
            case Rarity.Uncommon:
                color = uncommonColor;
                break;
            case Rarity.Common:
                color = commonColor;
                break;
        }
        return color;
    }
}