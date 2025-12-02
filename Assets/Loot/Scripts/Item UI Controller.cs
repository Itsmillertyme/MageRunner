//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class ItemUIController : MonoBehaviour
//{
//    [SerializeField] private Colors colorManager;
//    [SerializeField] TextMeshProUGUI titleTMP;
//    [SerializeField] Image itemIcon;
//    [SerializeField] Image decoration1;
//    [SerializeField] Image decoration2;
//    [SerializeField] TextMeshProUGUI buffsTMP;
//    [SerializeField] TextMeshProUGUI debuffsTMP;

//    public void Initialize(Rarity rarityIn, string titleIn, Sprite iconIn, string buffsIn, string debuffsIn)
//    {
//        //set statics
//        titleTMP.text = titleIn;
//        itemIcon.sprite = iconIn;
//        buffsTMP.text = buffsIn;
//        debuffsTMP.text = debuffsIn;

//        //set decoration color
//        Color itemRarityColor = GetColorBasedOnRarity(rarityIn);
//        decoration1.color = itemRarityColor;
//        decoration2.color = itemRarityColor;
//    }

//    private Color GetColorBasedOnRarity(Rarity itemRarity)
//    {
//        Color color;
//        switch (itemRarity)
//        {
//            case Rarity.Legendary: color = colorManager.LegendaryColor; break;
//            case Rarity.Exotic: color = colorManager.ExoticColor; break;
//            case Rarity.Rare: color = colorManager.RareColor; break;
//            case Rarity.Uncommon: color = colorManager.UncommonColor; break;
//            default: color = colorManager.CommonColor; break;
//        }
//        return color;
//    }
//}