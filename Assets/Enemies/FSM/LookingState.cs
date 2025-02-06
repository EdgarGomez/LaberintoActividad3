using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingState : State<EnemyController> {
    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);

        controller.navMeshAgent.updateRotation = false;
        controller.navMeshAgent.SetDestination(controller.transform.position);
    }

    public override void OnUpdateState() {
        Quaternion targetRotation = Quaternion.LookRotation(controller.getDirectionToPlayer().normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

        if (controller.DetectPlayer()) {
            controller.increaseAlertTimer();
        } else {
            controller.ChangeState(controller.patrolState);
        }
    }

    public override void OnExitState() {
        controller.navMeshAgent.updateRotation = true;
    }
}