using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MonsterUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    public void UpdateTitle(string newTitle)
    {
        title.SetText(newTitle);
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText.SetText( $"{currentHealth:F} / {maxHealth:F}");
    }
}
