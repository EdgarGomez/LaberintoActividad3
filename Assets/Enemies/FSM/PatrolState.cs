using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State<EnemyController> {
    [SerializeField] private Transform ruta;
    [SerializeField] private float stoppingDistanceRoute;

    private List<Vector3> points = new List<Vector3>();

    private int pointIndex;

    private void Start() {
        foreach (Transform point in ruta) {
            points.Add(point.transform.position);
        }

        pointIndex = 0;
    }


    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);
        controller.PlayWalkAnimation();
        controller.updateDetectInfo(0);
    }


    public override void OnUpdateState() {
        controller.navMeshAgent.SetDestination(points[pointIndex]);

        if (!controller.navMeshAgent.pathPending &&
            controller.navMeshAgent.remainingDistance <= stoppingDistanceRoute) {
            controller.ChangeState(controller.idleState);
        }
        
        if (controller.DetectPlayer()) {
            controller.ChangeState(controller.lookingState);
        } else {
            controller.decreaseAlertTimer();
        }
    }

    public override void OnExitState() {
        controller.PlayIdleAnimation();

    }

    public void updatePoint() {
        pointIndex++;
        pointIndex %= points.Count;
    }
}