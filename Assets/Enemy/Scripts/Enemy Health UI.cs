using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] Image healthBar;

    public void UpdateHealthBar(float value)
    {
        healthBar.fillAmount = value;
    }
}
