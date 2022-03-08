using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    public const float EffectiveMultiplier = 1.5f;
    public const float NormalDamageMultiplier = 1.0f;
    public const float IneffectiveMultiplier = 0.5f;
    
    [SerializeField] private string title;
    [SerializeField] private Element element;
    
    [Header("Attack")]
    [SerializeField] private string attackName;
    [SerializeField] private float attackStrength;
    [SerializeField] private GameObject attackEffect;

    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private UnityEvent onDamage;
    [SerializeField] private UnityEvent onFainted;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    #region Getter
    
    public string GetTitle()
    {
        return title;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
    #endregion

    private void ChangeHealth(float change)
    {
        currentHealth += change;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (change <= 0)
        {
            if (currentHealth > 0)
            {
                onDamage.Invoke();
            }
            else
            {
                onFainted.Invoke();
            }
        }
    }
    
    public string Attack(Monster enemy)
    {
        float attackMultiplier = GetDamageMultiplier(enemy.element);
        float damage = attackStrength * attackMultiplier;
        enemy.ChangeHealth(-damage);

        Instantiate(attackEffect, enemy.transform.position, Quaternion.identity);

        return GetAttackString(enemy, damage, attackMultiplier);
    }

    public bool HasFainted()
    {
        return currentHealth == 0;
    }

    private string GetAttackString(Monster enemy, float damage, float attackMultiplier)
    {
        string text = $"{title} wendet {attackName} an.";

        if (attackMultiplier > 1.01f)
        {
            text += " Es war sehr effektiv!";
        } 
        else if (attackMultiplier < 0.99f)
        {
            text += " Es war nicht sehr effektiv!";
        }

        text += $" {enemy.title} erleidet {damage} Schaden.";
        
        if (enemy.HasFainted())
        {
            text += $" {enemy.title} ist ohnmächtig geworden.";
        }

        return text;
    }
    
    private float GetDamageMultiplier(Element enemyElement)
    {
        switch (element)
        {
            case Element.Normal:
                return NormalDamageMultiplier;
            case Element.Fire:
                switch (enemyElement)
                {
                    case Element.Plant:
                        return EffectiveMultiplier;
                    case Element.Stone:
                    case Element.Water:
                        return IneffectiveMultiplier;
                    default:
                        return NormalDamageMultiplier;
                }
            case Element.Water:
                switch (enemyElement)
                {
                    case Element.Fire:
                        return EffectiveMultiplier;
                    case Element.Plant:
                        return IneffectiveMultiplier;
                    default:
                        return NormalDamageMultiplier;
                }
            case Element.Stone:
                switch (enemyElement)
                {
                    case Element.Normal:
                    case Element.Fire:
                        return EffectiveMultiplier;
                    case Element.Water:
                    case Element.Plant:
                        return IneffectiveMultiplier;
                    default:
                        return NormalDamageMultiplier;
                }
            case Element.Plant:
                switch (enemyElement)
                {
                    case Element.Stone:
                    case Element.Water:
                        return EffectiveMultiplier;
                    case Element.Fire:
                        return IneffectiveMultiplier;
                    default:
                        return NormalDamageMultiplier;
                }
                default:
                    return NormalDamageMultiplier;
        }
    }
}
