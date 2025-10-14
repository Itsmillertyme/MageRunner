using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    public void UpdateImageFill(float value)
    {
        healthBar.fillAmount = value;
    }
}