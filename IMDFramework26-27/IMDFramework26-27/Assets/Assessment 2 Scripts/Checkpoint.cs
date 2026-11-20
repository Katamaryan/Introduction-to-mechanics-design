using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public event Action OnPass;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            OnPass?.Invoke();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            OnPass?.Invoke();
    }
}
