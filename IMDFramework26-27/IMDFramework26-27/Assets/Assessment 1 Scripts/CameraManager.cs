using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Transform m_target;
    [SerializeField] private Vector3 m_targetPosition;
    [SerializeField] private float m_magnitude;
    [SerializeField] private float m_OffsetX;

    //x = bottom, y = midpoint, z = top
    [SerializeField] Vector3 m_movingBorder;
    private bool m_camLocked;
    [SerializeField] private float m_topBorderDist;
    [SerializeField] private float m_botBorderDist;


    private void LateUpdate()
    {
        m_magnitude = Vector3.Distance(transform.position, m_target.position);
       

        m_targetPosition = new Vector3(m_target.position.x + m_OffsetX, m_target.position.y, m_target.position.z);
        MovingVerticalBorder();
        m_targetPosition.y = m_movingBorder.y;

        if (!m_camLocked)
            transform.position = Vector3.Lerp(transform.position, m_targetPosition, m_magnitude * Time.deltaTime);
    }

    private void MovingVerticalBorder()
    {
        if (m_target.position.y > m_movingBorder.z)
        {
            float distance = m_target.position.y - m_movingBorder.z;

            m_movingBorder.y = m_movingBorder.y + distance;
            
        }
        if (m_target.position.y < m_movingBorder.x)
        {
            float distance = m_movingBorder.x - m_target.position.y;

            m_movingBorder.y = m_movingBorder.y - distance;
        }
        m_movingBorder.y = Mathf.Clamp(m_movingBorder.y, -2, 10);
        m_movingBorder.x = m_movingBorder.y - m_botBorderDist;
        m_movingBorder.z = m_movingBorder.y + m_topBorderDist;
    }

    public void StartDamageCameraShake()
    {
        StartCoroutine(CDamageCameraShake(0.1f, 0.1f));
    }

    public void StartLandingCameraShake(float yVelocity)
    {
        m_camLocked = true;
        StartCoroutine(CLandingCameraShake(0.1f * -yVelocity / 10, yVelocity));
    }


    private IEnumerator CLandingCameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0f;

        float y = 0f;

        while (elapsed < duration)
        {
            if (elapsed < duration / 2)
                y += Mathf.Clamp(y + Time.deltaTime, 0, magnitude);
            else
                y += Mathf.Clamp(y - Time.deltaTime, 0, magnitude);

            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(originalPos.x, originalPos.y + y, originalPos.z), Time.deltaTime);
        
            elapsed += Time.deltaTime;
            yield return null;
        }

        //Camera.main.transform.localPosition = originalPos;
        m_camLocked = false;
    }
   
    private IEnumerator CDamageCameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
        
            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }
}
