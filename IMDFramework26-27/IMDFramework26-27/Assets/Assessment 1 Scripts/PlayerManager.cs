using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private CharacterMovement m_characterMovement;
    private GroundCheck m_groundCheck;
    private InputHandler m_inputHandler;
    private ColliderPinch m_colliderPinch;
    private HealthComponent m_healthComponent;

    void Awake()
    {
        m_characterMovement = GetComponent<CharacterMovement>();
        m_colliderPinch = GetComponent<ColliderPinch>();
        m_groundCheck = GetComponent<GroundCheck>();
        m_inputHandler = GetComponent<InputHandler>();
        m_healthComponent = GetComponent<HealthComponent>();
    }

    private void Start()
    {
        m_healthComponent.onDamaged += Handle_OnHealthDamaged;
        m_healthComponent.onDead += Handle_OnPlayerDeath;
        m_inputHandler.onMovePerformed += m_characterMovement.MovePerformed;
        m_inputHandler.onMoveCanceled += m_characterMovement.SetInMove;
        m_inputHandler.onJumpPerformed += m_characterMovement.JumpPerformed;
        m_inputHandler.onJumpCanceled += m_characterMovement.JumpCanceled;
    }

    private void OnDestroy()
    {
        m_healthComponent.onDamaged -= Handle_OnHealthDamaged;
        m_healthComponent.onDead -= Handle_OnPlayerDeath;
        m_inputHandler.onMovePerformed -= m_characterMovement.MovePerformed;
        m_inputHandler.onMoveCanceled -= m_characterMovement.SetInMove;
        m_inputHandler.onJumpPerformed -= m_characterMovement.JumpPerformed;
        m_inputHandler.onJumpCanceled += m_characterMovement.JumpCanceled;
    }

    private void Handle_OnHealthDamaged(float currentHealth, float maxHealth, float inboundDamage)
    {
        Debug.Log($"Took: {inboundDamage} Damage | Current Health: {currentHealth} | Max Health: {maxHealth}");
    }

    private void Handle_OnPlayerDeath(MonoBehaviour causer)
    {
        Debug.Log($"bro is dead cus of {causer.gameObject.name}");

    }

   
}
