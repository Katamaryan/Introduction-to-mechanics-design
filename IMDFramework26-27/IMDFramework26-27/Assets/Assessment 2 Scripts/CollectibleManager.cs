using System;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public event Action<float> OnCollect; 
    [SerializeField] public GameObject[] m_collectibles;

    private float m_collectiblesMissed;
    private void Start()
    {
        CollectibleReset();

        for (int i = 0; i < m_collectibles.Length; i++)
        {
            m_collectibles[i].GetComponent<Collectible>().OnCollection += AddScore;
        }
    }
    private void AddScore(float scoreVal)
    {
        m_collectiblesMissed -= 1;
        OnCollect?.Invoke(m_collectiblesMissed);
    
    }
    public void CollectibleReset()
    {
        for (int i = 0; i < m_collectibles.Length; i++)
        {
            m_collectibles[i].GetComponent<Collectible>().ResetCollectible();
        
        }

        m_collectiblesMissed = m_collectibles.Length;
    }
}
