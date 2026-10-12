using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform m_target;
    private Vector2 m_currentPos;

    void FixedUpdate()
    {
        m_currentPos.x = m_target.position.x;
        transform.position = m_target.position;
    }

}
