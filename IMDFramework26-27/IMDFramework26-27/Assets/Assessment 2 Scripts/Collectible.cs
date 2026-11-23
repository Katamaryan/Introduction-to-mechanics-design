using System;
using UnityEngine;

public class Collectible : MonoBehaviour, IPickupable
{
    public event Action<float> OnCollection;
    [SerializeField] private float m_scoreValue;

    public void Collect()
    {
        OnCollection?.Invoke(m_scoreValue);
        AudioManager.instance.PlaySound(0);
        ParticleManager.instance.PlayParticle(4, transform.position, Vector3.zero);
        transform.gameObject.SetActive(false);
    }

    public void ResetCollectible()
    {
        transform.gameObject.SetActive(true);

    }
}