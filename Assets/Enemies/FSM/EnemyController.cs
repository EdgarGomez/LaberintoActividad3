using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : Controller {
    [SerializeField] private GameObject player;
    private PlayerHealth _playerHealth;

    private State<EnemyController> currentState;

    [NonSerialized] public PatrolState patrolState;
    [NonSerialized] public IdleState idleState;
    [NonSerialized] public AlertState alertState;
    [NonSerialized] public AttackState attackState;
    [NonSerialized] public ChaseState chaseState;
    [NonSerialized] public InvestigateState investigateState;
    [NonSerialized] public LookingState lookingState;

    [NonSerialized] public NavMeshAgent navMeshAgent;

    [SerializeField] private float visionRange;
    [SerializeField] private float closeRange;
    [SerializeField] private float visionAngle;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private float alertActivateTimer;
    private float detectTimer;

    [SerializeField] private GameObject detectInfo;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material alertMaterial;
    [SerializeField] private Material chaseMaterial;
    private MeshRenderer detectInfoMeshRenderer;

    [SerializeField] private RectTransform image;
    [SerializeField] private Image arrowImage;
    [SerializeField] private Camera _camera;

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        patrolState = GetComponent<PatrolState>();
        idleState = GetComponent<IdleState>();
        alertState = GetComponent<AlertState>();
        attackState = GetComponent<AttackState>();
        chaseState = GetComponent<ChaseState>();
        investigateState = GetComponent<InvestigateState>();
        lookingState = GetComponent<LookingState>();

        _playerHealth = player.GetComponent<PlayerHealth>();

        detectInfoMeshRenderer = detectInfo.GetComponent<MeshRenderer>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        GameManager.instance.onRespawn += resetBehaviour;

        ChangeState(patrolState);
    }

    private void OnDestroy()
    {
        GameManager.instance.onRespawn -= resetBehaviour;
    }

    private void resetBehaviour()
    {
        ChangeState(patrolState);
        detectTimer = 0;
    }

    void Update() {
        PointArrowAtEnemy();
        updateDetectArrow();
        if (currentState != null) {
            currentState.OnUpdateState();
        }
    }

    void FixedUpdate() {
        if (currentState != null) {
            currentState.OnFixedUpdateState();
        }
    }

    public void ChangeState(State<EnemyController> state) {
        Debug.Log(state.ToString());
        if (currentState != null) {
            currentState.OnExitState();
        }

        currentState = state;
        currentState.OnEnterState(this);
    }

    public bool DetectPlayer() {
        Collider[] closeColls = Physics.OverlapSphere(transform.position, closeRange, playerLayer);
        if (closeColls.Length > 0) return true;
        Collider[] colls = Physics.OverlapSphere(transform.position, visionRange, playerLayer);
        if (colls.Length <= 0) return false;
        Vector3 targetDirection = (colls[0].transform.position - transform.position);
        if (!(Vector3.Angle(transform.forward, targetDirection) < (visionAngle / 2f))) return false;
        return !Physics.Raycast(transform.position, targetDirection, targetDirection.magnitude, obstacleLayer);
    }

    public bool SeePlayer() {
        if (!(Vector3.Angle(transform.forward, getDirectionToPlayer()) < (visionAngle / 2f))) return false;
        return !Physics.Raycast(transform.position, getDirectionToPlayer().normalized, getDirectionToPlayer().magnitude,
            obstacleLayer);
    }

    public void increaseAlertTimer() {
        Vector3 targetDirection = getDirectionToPlayer();
        float targetDistance = targetDirection.magnitude;
        detectTimer += Time.deltaTime * 15 / targetDistance;
        if (detectTimer >= alertActivateTimer) {
            ChangeState(alertState);
        }
    }

    public void decreaseAlertTimer() {
        detectTimer -= (Time.deltaTime / 2f);
        detectTimer = Mathf.Max(0, detectTimer);
    }

    public Vector3 getDirectionToPlayer() {
        return player.transform.position - transform.position;
    }

    public Vector3 getPlayerPosition() {
        return player.transform.position;
    }

    public PlayerHealth getPlayerHealth() {
        return _playerHealth;
    }

    public void updateDetectInfo(int level) {
        switch (level) {
            case 0: // Invisible
                detectInfoMeshRenderer.enabled = false;
                arrowImage.color = new Color(255, 255, 255, 100);
                break;
            case 1: // Normal
                detectInfoMeshRenderer.enabled = true;
                detectInfoMeshRenderer.material = normalMaterial;
                arrowImage.color = new Color(255, 255, 255, 100);
                break;
            case 2: // Alert
                detectInfoMeshRenderer.enabled = true;
                detectInfoMeshRenderer.material = alertMaterial;
                arrowImage.color = new Color(255, 244, 0, 150);
                break;
            case 3: // Chase
                detectInfoMeshRenderer.enabled = true;
                detectInfoMeshRenderer.material = chaseMaterial;
                arrowImage.color = new Color(200, 0, 0, 150);
                break;
        }
    }

    private void PointArrowAtEnemy() {
        Vector3 playerForward = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z).normalized;
        Vector3 directionToPlayer = -getDirectionToPlayer().normalized;
        float angle = Vector3.SignedAngle(playerForward, directionToPlayer, Vector3.down);
        image.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void updateDetectArrow() {
        arrowImage.fillAmount = detectTimer / alertActivateTimer;
    }

    public void PlayIdleAnimation()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", false);
    }

    public void PlayWalkAnimation()
    {
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", false);
    }

    public void PlayRunAnimation()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttacking", false);
    }

    public void PlayAttackAnimation()
    {
        animator.SetBool("IsAttacking", true);
    }
}