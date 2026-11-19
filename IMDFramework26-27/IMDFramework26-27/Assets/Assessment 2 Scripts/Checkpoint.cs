using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public event Action OnPass;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            OnPass?.Invoke();
    }
}
