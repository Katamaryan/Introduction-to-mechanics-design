using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    #region Event definitions

    public event Action<float> onMovePerformed;
    public event Action<float> onMoveCanceled;
    public event Action onJumpPerformed;
    public event Action onJumpCanceled;
   // public event Action onDropPerformed;

    #endregion
    private PlayerControls m_ActionMap;
    
    #region Bindings
    private void OnEnable()
    {
        m_ActionMap.Enable();

        m_ActionMap.Default.MoveHoriz.performed += Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled += Handle_MoveCancelled;
        m_ActionMap.Default.Jump.started += Handle_JumpPerformed;
        m_ActionMap.Default.Jump.canceled += Handle_JumpCanceled;
        m_ActionMap.Default.Interact.performed += Handle_InteractionPerformed;
        m_ActionMap.Default.Drop.performed += Handle_DropPerformed;
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Jump.started -= Handle_JumpPerformed;
        m_ActionMap.Default.Jump.canceled -= Handle_JumpCanceled;
        m_ActionMap.Default.Interact.performed -= Handle_InteractionPerformed;
        m_ActionMap.Default.Drop.performed -= Handle_DropPerformed;
    }
    #endregion

    [SerializeField] private LayerMask m_InteractLayer;
    [SerializeField] private LayerMask m_PlatformLayer;

    private void Awake()
    {
        m_ActionMap = new PlayerControls();
    }
    
    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        //adding the question mark makes it so the code checks if the event is null be invoking and if is null then skips it
        onMovePerformed?.Invoke(context.ReadValue<float>());
    }

    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        onMoveCanceled?.Invoke(0);
    }

    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        onJumpPerformed?.Invoke();
    }

    private void Handle_JumpCanceled(InputAction.CallbackContext context)
    {
        onJumpCanceled?.Invoke();
    }

    private void Handle_InteractionPerformed(InputAction.CallbackContext context)
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 1, m_InteractLayer);

        if (col != null && col.transform.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.Interact();
        }
    }

    private void Handle_DropPerformed(InputAction.CallbackContext context)
    {
        //onDropPerformed?.Invoke();

        Collider2D col = Physics2D.OverlapCircle(transform.GetChild(0).position, 1, m_PlatformLayer);

        if (col != null)
        {
            BoxCollider2D platCol = col.GetComponent<BoxCollider2D>();
            BoxCollider2D playerCol = transform.GetChild(2).GetComponent<BoxCollider2D>();
            StartCoroutine(DropPlatform(platCol, playerCol));
        }
    }

    IEnumerator DropPlatform(BoxCollider2D platCol, BoxCollider2D playerCol)
    {
        //Debug.Log("goes through");
        Physics2D.IgnoreCollision(playerCol, platCol, true);
        //Debug.LogFormat("ignoring collision between {0}. {1} is bool: {2}", playerCol.gameObject.name, platCol.gameObject.name, Physics2D.GetIgnoreCollision(playerCol, platCol));
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCol, platCol, false);

    }
}
