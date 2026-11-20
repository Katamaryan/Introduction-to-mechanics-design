using System;
using System.Collections;
using UnityEngine;

public class CameraEaseOut : MonoBehaviour
{
    public event Action<float> OnEaseOut;
    public event Action OnEnter;

    public float m_size;
    [SerializeField] private float m_maxSize;
    public float m_defaultSize;

    void Start()
    {
        m_size = m_defaultSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnEnter?.Invoke();
            StartCoroutine(CCamEaseOut());
        }
    }

    private IEnumerator CCamEaseOut()
    {

        while (m_size < m_maxSize)
        {
            m_size += Time.deltaTime;
            OnEaseOut?.Invoke(m_size);
            yield return new WaitForFixedUpdate();
        }


    }
}
