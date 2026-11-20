using System;
using System.Collections;
using UnityEngine;

public class CameraEaseIn : MonoBehaviour
{
    public event Action<float> OnEaseIn;
    public event Action OnEnter;
    public float m_size;
    public float m_defaultSize;
    [SerializeField] private float m_duration;
    
    void Start()
    {
        m_size = m_defaultSize;    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnEnter?.Invoke();
            StartCoroutine(CCamEaseIn());
        }
    }

    private IEnumerator CCamEaseIn()
    {
        
        while (m_size > m_defaultSize)
        {
            m_size -= Time.deltaTime; 
            OnEaseIn?.Invoke(m_size);
            yield return new WaitForFixedUpdate();
        }

    }
}
