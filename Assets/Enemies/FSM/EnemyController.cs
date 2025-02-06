using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Controller {
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


    void Start() {
        patrolState = GetComponent<PatrolState>();
        idleState = GetComponent<IdleState>();
        alertState = GetComponent<AlertState>();
        attackState = GetComponent<AttackState>();
        chaseState = GetComponent<ChaseState>();
        investigateState = GetComponent<InvestigateState>();
        lookingState = GetComponent<LookingState>();

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
        if (closeColls.Length > 0) {
            return true;
        }

        Collider[] colls = Physics.OverlapSphere(transform.position, visionRange, playerLayer);
        if (colls.Length <= 0) return false;
        Vector3 targetDirection = (colls[0].transform.position - transform.position);
        Debug.DrawRay(transform.position, targetDirection.normalized * visionRange, Color.green);
        if (!(Vector3.Angle(transform.forward, targetDirection) < (visionAngle / 2f))) return false;
        return !Physics.Raycast(transform.position, targetDirection, targetDirection.magnitude, obstacleLayer);
    }

    public void increaseAlertTimer() {
        Vector3 targetDirection = getDirectionToPlayer();
        float targetDistance = targetDirection.magnitude;
        detectTimer += Time.deltaTime * 6 / targetDistance;
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
        Collider[] colls = Physics.OverlapSphere(transform.position, visionRange, playerLayer);
        if (colls.Length <= 0) return Vector3.zero;
        return colls[0].transform.position - transform.position;
    }
}