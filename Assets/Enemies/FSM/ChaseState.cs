using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : State<EnemyController> {
    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);

        Debug.Log("Chasing!");
        controller.navMeshAgent.speed *= 3f;
        
        controller.updateDetectInfo(3);
    }

    public override void OnUpdateState() {
        controller.navMeshAgent.SetDestination(controller.getPlayerPosition());

        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance) {
            controller.ChangeState(controller.attackState);
        }

        if (!controller.DetectPlayer()) {
            controller.ChangeState(controller.investigateState);
        }
    }

    public override void OnExitState() {
        controller.navMeshAgent.speed /= 3f;
    }
}