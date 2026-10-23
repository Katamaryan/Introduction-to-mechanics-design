using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_CharacterMovement;

    #region Event definitions

    public event Action<float> onMovePerformed;
    public event Action<float> onMoveCanceled;
    public event Action onJumpPerformed;
    public event Action onJumpCanceled;
    public event Action onDropPerformed;

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
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Jump.started -= Handle_JumpPerformed;
        m_ActionMap.Default.Jump.canceled -= Handle_JumpCanceled;
    }
    #endregion

    private void Awake()
    {
        m_ActionMap = new PlayerControls();

    }
    
    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        onMovePerformed?.Invoke(context.ReadValue<float>());
    }

    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        onMoveCanceled?.Invoke(0);
    }

    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        //adding the question mark makes it so the code checks if the event is null be invoking and if is null then skips it
        onJumpPerformed?.Invoke();
    }

    private void Handle_JumpCanceled(InputAction.CallbackContext context)
    {
        onJumpCanceled?.Invoke();
    }

    private void Handle_DropPerformed()
    {
        onDropPerformed?.Invoke();
    }
}
