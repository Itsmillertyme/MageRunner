using UnityEngine;

[CreateAssetMenu(menuName = "Settings/UI Colors")]

public class UIColorManager : ScriptableObject
{
    [SerializeField] Color legendaryColor;
    [SerializeField] Color exoticColor;
    [SerializeField] Color rareColor;
    [SerializeField] Color uncommonColor;
    [SerializeField] Color commonColor;

    public Color LegendaryColor => legendaryColor;
    public Color ExoticColor => exoticColor;
    public Color RareColor => rareColor;
    public Color UncommonColor => uncommonColor;
    public Color CommonColor => commonColor;
}