using UnityEngine;
using UnityEngine.Pool;

public class ParticleObjectPool
{
    GameObject m_particlePrefab;
    ObjectPool<GameObject> particlePool;

    public ParticleObjectPool( GameObject prefab , int defaultSize, int maxSize)
    {
        this.m_particlePrefab = prefab;
        particlePool = new ObjectPool<GameObject>(
            CreatePooledParticle,
            OnGetFromPool,
            OnReturnToPool,
            OnDestroyPooledObject,
            true,
            defaultSize,
            maxSize
            );
    }

    //creates new particle object for the pool when it is empty
    GameObject CreatePooledParticle()
    {
        GameObject newParticle = GameObject.Instantiate( m_particlePrefab );
        return newParticle;
    }
    public GameObject GetObject(Vector3 position, Vector3 rotation)
    {
        GameObject particle = particlePool.Get();
        particle.transform.position = position;
        particle.transform.rotation = Quaternion.Euler(rotation);
        return particle;
    }

    public void ReleaseObject(GameObject pooledParticle)
    {
        particlePool.Release(pooledParticle);
    }

    //activates particle object when the pool gets it
    void OnGetFromPool(GameObject pooledParticle)
    {
        pooledParticle.GetComponent<ParticleDespawn>().OnDespawn += ReleaseObject;
        pooledParticle.SetActive(true);
    }

    void OnReturnToPool(GameObject pooledParticle)
    {
        pooledParticle.GetComponent<ParticleDespawn>().OnDespawn -= ReleaseObject;
        pooledParticle.SetActive(false);
    }

    void OnDestroyPooledObject(GameObject pooledParticle)
    {
        GameObject.Destroy(pooledParticle);
    }

}
