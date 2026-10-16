using Unity.VisualScripting;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float m_damage;
     private void OnTriggerEnter2D(Collider2D collision)
     {
        if (collision.transform.parent.TryGetComponent<HealthComponent>(out var healthComponent))
        {
            healthComponent.ApplyDamage(m_damage, this);
        }
        
     }
}
