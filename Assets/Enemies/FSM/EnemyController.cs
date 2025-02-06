using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Controller
{
    private State<EnemyController> currentState;

    private PatrolState patrolState;
    private IdleState idleState;

    #region getters
    public PatrolState getPatrolState { get => patrolState; }
    public IdleState getIdleState { get => idleState; }
    #endregion

    public NavMeshAgent navMeshAgent;

    void Start()
    {
        patrolState = GetComponent<PatrolState>();
        idleState = GetComponent<IdleState>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        ChangeState(patrolState);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdateState();
        }
    }

    public void ChangeState(State<EnemyController> state)
    {
        if (currentState != null)
        {
            currentState.OnExitState();
        }
        currentState = state;
        currentState.OnEnterState(this);
    }
}
