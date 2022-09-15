using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
    
{
    public const float NormalDamageMultiplier = 1.0f;
    public const float EffectiveDamageMultiplier = 1.5f;
    public const float IneffectiveDamageMultiplier = 0.5f;

    [SerializeField] private string title = "Monster";
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private ElementInit attackElement = Element.Normal;
    [Space]
    [Header("Attack")]

    [SerializeField] private string attackName = "ATTACKENNAME";
    [SerializeField] private float attackStrengh = 2f;
    [SerializeField] private GameObject attackEffect;

    [SerializeField] private UnityEvent onDamaged;
    [SerializeField] private UnityEvent onFainted;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public string Attack(Monster target) 
    {
        float damage =this.attackStrengh;
        float attackMultiplier;
        float damage = this.attackHealth(-damage);
        target.UpdateHealth(-damage);
        Instantiate(attackEffect,target.transform.transform.position, Quaternion.identity);
        return GetAttackString(target,damage);
    }
    
    public bool HasFainted() { }
   
    private void UpdateHealth(float change)
    {
        float newHealth = currentHealth + change;
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        if(change < 0)
        {
            if(currentHealth > 0)
            {
                onDamaged.Invoke();
            }
            else
            {
                onFainted.Invoke();
            }
        }
    }
    private string GetAttackString(Monster target,float damage)
    {
        string text = $"{this.title} wendet {this.attackName} an.";
        text += $" {target.title} erleidet {damage:F1} Schaden.";
        //TODO add
        if (target.HasFainted())
        {
            text += $" {target.title} ist ohnmächtig geworden.";
        }
        return text;
    }

    public string GetTitle()
    {
        return title;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
