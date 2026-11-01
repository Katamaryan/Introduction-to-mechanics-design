using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour, IDroppable
{
    [SerializeField] private LayerMask m_playerLayer;
    private BoxCollider2D m_collider;
    Coroutine C_disableCollider;
    private bool m_coroutineActive;
    private void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }
    public void Drop()
    {
        Debug.Log("bum");
        if (m_coroutineActive) { return; }
        m_coroutineActive = true;
        C_disableCollider = StartCoroutine(DisableCollision());
        
    }

    private IEnumerator DisableCollision()
    {
        Debug.Log("poo");
        Collider2D col = Physics2D.OverlapBox(transform.position + Vector3.up, new Vector2(2, 2), 0f, m_playerLayer);
        if (col == null)
        {
            Debug.Log("fuck you");

        }
        else
        {
            BoxCollider2D playerCol = col.GetComponent<BoxCollider2D>();
            if (playerCol == null)
            {
                Debug.Log("fuck you");

            }
            Physics2D.IgnoreCollision(m_collider, playerCol, true);
            yield return new WaitForSeconds(.25f);
            Physics2D.IgnoreCollision(m_collider, playerCol, false);
            m_coroutineActive = false;
        }
    }
}
