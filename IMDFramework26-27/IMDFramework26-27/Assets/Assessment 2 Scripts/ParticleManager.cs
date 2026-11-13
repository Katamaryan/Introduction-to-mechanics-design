using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_particlePrefab = new GameObject[3];
    private GameObject[] m_particleRef = new GameObject[3];
    private ParticleObjectPool[] m_particlePool = new ParticleObjectPool[3];
    public static ParticleManager instance { get; set; }

    [SerializeField] private int m_defaultSize;
    [SerializeField] private int m_MaxSize;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        for (int i = 0; i < m_particlePrefab.Length; i++)
        {
            m_particleRef[i] = Instantiate(m_particlePrefab[i]);
            m_particleRef[i].SetActive(false);
            //  m_particlePool[i] = new ParticleObjectPool(m_particlePrefab[i], m_defaultSize, m_MaxSize);
        }
    }

    public void SpawnParticle(int particleID, Vector3 position, Vector3 rotation)
    {
        //m_particlePool[particleID].GetObject(position, rotation);
        m_particleRef[particleID].SetActive(true);
        m_particleRef[particleID].transform.position = position;
        m_particleRef[particleID].transform.rotation = Quaternion.Euler(rotation);

    }
    public void PlayParticle(int particleID, Vector3 position, Vector3 rotation)
    {
        //m_particlePool[particleID].GetObject(position, rotation);
        m_particleRef[particleID].SetActive(true);
        m_particleRef[particleID].GetComponent<ParticleSystem>().Play();
        m_particleRef[particleID].transform.position = position;
        m_particleRef[particleID].transform.rotation = Quaternion.Euler(rotation);

    }



    public void DespawnParticle(int particleID)
    {
        m_particleRef[particleID].SetActive(false);
    }
}
