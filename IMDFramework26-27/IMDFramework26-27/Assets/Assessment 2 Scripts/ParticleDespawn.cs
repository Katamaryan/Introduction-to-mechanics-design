using System;
using System.Collections;
using UnityEngine;

public class ParticleDespawn : MonoBehaviour
{
    public event Action<GameObject> OnDespawn;

    private Coroutine C_DespawnTimer;
    private bool m_despawnActive;
    [SerializeField] private float m_lifeSpan;

    private void OnEnable()
    {
        m_despawnActive = true;
        C_DespawnTimer = StartCoroutine(DespawnTimer());
    }

    private IEnumerator DespawnTimer()
    {
        while ( m_despawnActive)
        yield return new WaitForSeconds(m_lifeSpan);

        Debug.Log("im doing shit");
        OnDespawn?.Invoke(this.gameObject);
        m_despawnActive = false;
    }
}
