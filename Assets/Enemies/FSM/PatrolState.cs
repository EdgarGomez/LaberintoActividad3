using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State<EnemyController>
{
    [SerializeField] private Transform ruta;

    private List<Vector3> points = new List<Vector3>();

    private int pointIndex;

    private void Start()
    {
        foreach (Transform point in ruta)
        {
            points.Add(point.transform.position);
        }
        pointIndex = 0;
    }


    public override void OnEnterState(EnemyController controller)
    {
        base.OnEnterState(controller);
    }


    public override void OnUpdateState()
    {
        controller.navMeshAgent.SetDestination(points[pointIndex]);

        if (!controller.navMeshAgent.pathPending && controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance)
        {
            controller.ChangeState(controller.getIdleState);
        }
    }

    public override void OnExitState()
    {
        pointIndex++;
        pointIndex %= points.Count;
    }
}
