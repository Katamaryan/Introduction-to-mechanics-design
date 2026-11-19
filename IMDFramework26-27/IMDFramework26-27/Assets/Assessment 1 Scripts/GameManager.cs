using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerPrefab;
    private GameObject m_playerRef;
    [SerializeField] private TimeManager m_timeManager;
    [SerializeField] private CollectibleManager m_collectibleManager;
    [SerializeField] private GameObject m_cameraRef;
    private Transform m_cameraFollowerRef;

    void Awake()
    {
        m_playerRef = Instantiate(m_PlayerPrefab);

        m_playerRef.GetComponent<PlayerManager>().init();
        //gets camerafollower
        m_cameraFollowerRef = m_playerRef.transform.GetChild(3);
        m_cameraRef.GetComponent<CameraManager>().m_target = m_cameraFollowerRef;

        m_timeManager.m_maxCollectibles = m_collectibleManager.m_collectibles.Length;
    }

    void OnEnable()
    {
        m_playerRef.GetComponent<PlayerManager>().OnSpawn += EnablePlayer;

        m_collectibleManager.OnCollect += m_timeManager.CollictibleGot;
        m_timeManager.OnRestart += m_collectibleManager.CollectibleReset;
    }

    private void EnablePlayer()
    { 
        m_playerRef.GetComponent<PlayerManager>().OnDamage += m_cameraRef.GetComponent<CameraManager>().StartCameraShake;
        m_playerRef.GetComponent<PlayerManager>().OnLand += m_cameraRef.GetComponent<CameraManager>().StartCameraShake;
        m_playerRef.GetComponent<PlayerManager>().OnMove += m_cameraRef.GetComponent<CameraManager>().SetHorizontalOffset;
        m_playerRef.GetComponent<PlayerManager>().OnSpawn += DisablePlayer;
    }
    
    private void DisablePlayer()
    { 
        m_playerRef.GetComponent<PlayerManager>().OnDamage -= m_cameraRef.GetComponent<CameraManager>().StartCameraShake;
        m_playerRef.GetComponent<PlayerManager>().OnLand -= m_cameraRef.GetComponent<CameraManager>().StartCameraShake;
        m_playerRef.GetComponent<PlayerManager>().OnMove -= m_cameraRef.GetComponent<CameraManager>().SetHorizontalOffset;
        m_playerRef.GetComponent<PlayerManager>().OnSpawn -= EnablePlayer;
        m_playerRef.GetComponent<PlayerManager>().OnSpawn -= DisablePlayer;
    }

}