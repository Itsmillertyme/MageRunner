using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Item Perks/Damage Resistance")]

public class PerkDamageResistance : ItemPerk
{
    private float amountToChange;

    public override void OnEnable()
    {
        // DO NOTHING
    }

    public override void Add(float amount, int sign)
    {
        amountToChange = player.DamageResistance * amount;

        if (sign == 0)  // SET SIGN VALUE. 0 FOR POSITIVE, 1 FOR NEGATIVE.
        {

            player.SetDamageResistance(amountToChange);
        }
        else
        {
            player.SetDamageResistance(-amountToChange);
        }
    }

    public override void Remove()
    {
        player.SetDamageResistance(-amountToChange);
    }
}