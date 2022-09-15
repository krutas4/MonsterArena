using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Slider helthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    public void UpdateTitle(string newTitle)
    {
        titleText.SetText(newTitle);
    }
    public void UpdateHealth(float currentHealth,float maxHealth)
    {
        helthSlider.maxValue = maxHealth;
        helthSlider.value = currentHealth;
        string newHealthText = $"{currentHealth:F1}/{maxHealth:F1}";
        healthText.SetText(newHealthText);
    }
}
