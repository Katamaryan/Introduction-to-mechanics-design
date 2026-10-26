using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerPrefab;
    private GameObject m_playerRef;
    private Transform m_cameraFollowerRef;
    [SerializeField] private GameObject m_CameraPrefab; 
    [SerializeField] private GameObject m_cameraRef;

    void Awake()
    {
        m_playerRef = Instantiate(m_PlayerPrefab);
        //m_cameraRef = Instantiate(m_CameraPrefab);

        m_playerRef.GetComponent<PlayerManager>().init();
        //gets camerafollower
        m_cameraFollowerRef = m_playerRef.transform.GetChild(3);
        m_cameraRef.GetComponent<CameraManager>().m_target = m_cameraFollowerRef;
    }
}