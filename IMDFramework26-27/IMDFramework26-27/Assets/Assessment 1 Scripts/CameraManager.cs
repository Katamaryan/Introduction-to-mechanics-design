using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform m_target;

    private void Start()
    {
    }
    private void FixedUpdate()
    { 
        transform.position = Vector3.Lerp(transform.position, m_target.position, 10);
    }

}
