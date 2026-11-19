using System;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public event Action<float> OnCollect;

    public void UpdateScore(float scoreVal)
    {
        OnCollect?.Invoke(scoreVal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IPickupable>(out var collectible))
        {
            collectible.Collect();
        }
    }
}
