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
    [SerializeField] private Vector2 m_camLimits;


    private void LateUpdate()
    {
        m_magnitude = Vector3.Distance(transform.position, m_target.position);
       

        m_targetPosition = new Vector3(m_target.position.x + m_OffsetX, m_target.position.y, m_target.position.z);
        MovingVerticalBorder();
        m_targetPosition.y = m_movingBorder.y;;

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
        m_movingBorder.y = Mathf.Clamp(m_movingBorder.y, m_camLimits.x, m_camLimits.y);
        m_movingBorder.x = m_movingBorder.y - m_botBorderDist;
        m_movingBorder.z = m_movingBorder.y + m_topBorderDist;
    }

    public void SetHorizontalOffset(float direction)
    {
        m_OffsetX  = direction * 5;
    }

    public void StartCameraShake(float yVelocity)
    {
        if (yVelocity == 0)
        {
            StartCoroutine(CCameraShake(.5f, .2f));
        }
        //m_camLocked = true;
        else
            StartCoroutine(CCameraShake(.1f, .1f * -yVelocity / 10));
    }
   
    private IEnumerator CCameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1, 1f) * magnitude / Mathf.Exp(elapsed * 2);
            float y = UnityEngine.Random.Range(-1, 1f) * magnitude / Mathf.Exp(elapsed * 4);

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
        
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        //m_camLocked = false;
    }
}
