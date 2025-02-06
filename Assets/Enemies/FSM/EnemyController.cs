using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Controller {
    [SerializeField] private GameObject player;
    private PlayerHealth _playerHealth;
    
    private State<EnemyController> currentState;

    public PatrolState patrolState;
    public IdleState idleState;
    public AlertState alertState;
    public AttackState attackState;
    public ChaseState chaseState;
    public InvestigateState investigateState;
    public LookingState lookingState;

    public NavMeshAgent navMeshAgent;

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

    void Start() {
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

        ChangeState(patrolState);
    }

    void Update() {
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

    public void increaseAlertTimer() {
        Vector3 targetDirection = getDirectionToPlayer();
        float targetDistance = targetDirection.magnitude;
        detectTimer += Time.deltaTime * 15 / targetDistance;
        Debug.Log("Detect: " + detectTimer);
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
                break;
            case 1: // Normal
                detectInfoMeshRenderer.enabled = true;
                detectInfoMeshRenderer.material = normalMaterial;
                break;
            case 2: // Alert
                detectInfoMeshRenderer.enabled = true;
                detectInfoMeshRenderer.material = alertMaterial;
                break;
            case 3: // Chase
                detectInfoMeshRenderer.enabled = true;
                detectInfoMeshRenderer.material = chaseMaterial;
                break;
        }
    }
}