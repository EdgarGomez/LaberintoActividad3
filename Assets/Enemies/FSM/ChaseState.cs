using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : State<EnemyController> {
    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);
        
        controller.PlayRunAnimation();
        controller.navMeshAgent.speed *= 3f;
        controller.navMeshAgent.updateRotation = false;
        
        controller.updateDetectInfo(3);
    }

    public override void OnUpdateState() {
        controller.navMeshAgent.SetDestination(controller.getPlayerPosition());
        
        Quaternion targetRotation = Quaternion.LookRotation(controller.getDirectionToPlayer().normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

        if (!controller.navMeshAgent.pathPending && controller.navMeshAgent.remainingDistance <= (controller.navMeshAgent.stoppingDistance + 1.5f)) {
            controller.ChangeState(controller.attackState);
        }

        if (!controller.SeePlayer()) {
            controller.ChangeState(controller.investigateState);
        }
    }

    public override void OnExitState() {
        controller.navMeshAgent.speed /= 3f;
        controller.navMeshAgent.updateRotation = true;
        controller.PlayIdleAnimation();
    }
}