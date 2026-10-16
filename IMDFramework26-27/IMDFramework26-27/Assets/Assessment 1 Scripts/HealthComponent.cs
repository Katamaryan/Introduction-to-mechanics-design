using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float, float, float> onDamaged;
    public event Action<MonoBehaviour> onDead;

    [SerializeField] private float m_maxHealth;
    private float m_currentHealth;

    void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    public void ApplyDamage(float damage,MonoBehaviour causer)
    {
        //prevents health from going below 0 as whaen damage is less than current health then damage is taken from health but if current health is lower than damage then it takes the rest of health away instead
        float change = Mathf.Min(m_currentHealth, damage);
        m_currentHealth -= change;

        //think this is like the same as broadcast n unreal where this sends out the signal to listener functions bound to this event
        onDamaged?.Invoke(m_currentHealth, m_maxHealth, damage);
        if (m_currentHealth == 0) onDead?.Invoke(causer);
    }

}
