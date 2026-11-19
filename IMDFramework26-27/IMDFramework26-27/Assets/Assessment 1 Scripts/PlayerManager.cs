using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Component references
    private CharacterMovement m_characterMovement;
    private GroundCheck m_groundCheck;
    private InputHandler m_inputHandler;
    private ColliderPinch m_colliderPinch;
    private HealthComponent m_healthComponent;
    private PlayerAnimation m_playerAnimation;
    private Rigidbody2D m_RB;
    #endregion

    public event Action<float> OnLand;
    public event Action<float> OnMove;
    public event Action<float> OnDamage;
    public event Action OnSpawn;
    public event Action OnDespawn;
    private bool m_respawnActive = false;
    private Coroutine m_CRespawn;

    void Awake()
    {
        m_characterMovement = transform.GetComponent<CharacterMovement>();
        m_colliderPinch = transform.GetChild(2).GetComponent<ColliderPinch>();
        m_groundCheck = transform.GetChild(0).GetComponent<GroundCheck>();
        m_inputHandler = transform.GetComponent<InputHandler>();
        m_healthComponent = transform.GetComponent<HealthComponent>();
        m_playerAnimation = transform.GetChild(1).GetComponent<PlayerAnimation>();
        m_RB = transform.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        OnSpawn?.Invoke();
        m_healthComponent.onDamaged += Handle_OnHealthDamaged;
        m_healthComponent.onDead += Handle_OnPlayerDeath;
        m_inputHandler.onMovePerformed += m_characterMovement.MovePerformed;
        m_inputHandler.onMoveCanceled += m_characterMovement.SetInMove;
        m_inputHandler.onJumpPerformed += m_characterMovement.JumpPerformed;
        m_inputHandler.onJumpCanceled += m_characterMovement.JumpCanceled;
        m_characterMovement.OnJumpStateChange += m_colliderPinch.Pinch;
        m_characterMovement.OnAnimationParamChange += m_playerAnimation.AnimationParamChanged;
        m_characterMovement.OnLand += Handle_OnLand;
        m_characterMovement.OnMove += Handle_OnMove;
        m_groundCheck.onGrounded += m_characterMovement.OnGrounded;
    }

    private void OnDestroy()
    {
        OnDespawn?.Invoke();
        m_healthComponent.onDamaged -= Handle_OnHealthDamaged;
        m_healthComponent.onDead -= Handle_OnPlayerDeath;
        m_inputHandler.onMovePerformed -= m_characterMovement.MovePerformed;
        m_inputHandler.onMoveCanceled -= m_characterMovement.SetInMove;
        m_inputHandler.onJumpPerformed -= m_characterMovement.JumpPerformed;
        m_inputHandler.onJumpCanceled -= m_characterMovement.JumpCanceled;
        m_characterMovement.OnJumpStateChange -= m_colliderPinch.Pinch;
        m_characterMovement.OnAnimationParamChange -= m_playerAnimation.AnimationParamChanged;
        m_characterMovement.OnLand -= Handle_OnLand;
        m_characterMovement.OnMove -= Handle_OnMove;
        m_groundCheck.onGrounded -= m_characterMovement.OnGrounded;
    }

    
    private void Handle_OnLand(float yVel)
    {
        OnLand?.Invoke(yVel);
    }
    
    private void Handle_OnMove(float direction)
    {
        OnMove?.Invoke(direction);
    }
    
    private void Handle_OnHealthDamaged(float currentHealth, float maxHealth, float inboundDamage)
    {
        Debug.Log($"Took: {inboundDamage} Damage | Current Health: {currentHealth} | Max Health: {maxHealth}");
        OnDamage?.Invoke(0f);
    }

    private void Handle_OnPlayerDeath(MonoBehaviour causer)
    {
        Debug.Log($"bro is dead cus of {causer.gameObject.name}");

        if (m_respawnActive) { return; }

        m_respawnActive = true;
        m_CRespawn = StartCoroutine(C_PlayerRespawn());
    }

    public void init()
    {
        Debug.Log("Spawned in from the Game Manager");
    }

    IEnumerator C_PlayerRespawn()
    {
        while (m_respawnActive)
        {
            m_RB.AddForce(Vector3.up * (m_RB.linearVelocityY + 5) * 2, ForceMode2D.Impulse);
            m_playerAnimation.AnimationDamage(m_characterMovement.m_lastMoveDirection, true);
            m_inputHandler.m_ActionMap.Disable();
            ParticleManager.instance.PlayParticle(3, transform.position, Vector3.zero);
            PostProcessingManager.instance.ActivateEffect(0);
            Time.timeScale = .3f;

            yield return new WaitForSeconds(.5f);

            Time.timeScale = 1.0f;
            PostProcessingManager.instance.DeactivateEffect(0);
            m_playerAnimation.AnimationDamage(m_characterMovement.m_lastMoveDirection, false);
            m_inputHandler.m_ActionMap.Enable();
            transform.position = m_groundCheck.m_safePosition;
            m_respawnActive = false;
        }
    }
}
