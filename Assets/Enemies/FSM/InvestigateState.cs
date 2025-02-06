using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvestigateState : State<EnemyController> {
    private Vector3 lastKnownPosition;

    private Coroutine lookAroundCoroutine;
    private bool isLookingAround;

    private Quaternion rotationObjective;
    private bool enableRotation;
    
    
    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);

        Debug.Log("Investigating...");
        lastKnownPosition = controller.getPlayerPosition();
        enableRotation = false;
        
        controller.updateDetectInfo(2);
    }

    public override void OnUpdateState() {
        controller.navMeshAgent.SetDestination(lastKnownPosition);

        if (enableRotation) {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationObjective, Time.deltaTime * 10);
        }
        
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !isLookingAround) {
            lookAroundCoroutine = StartCoroutine(LookAround());
        }
        
        if (controller.DetectPlayer()) {
            controller.ChangeState(controller.chaseState);
        }
    }

    public override void OnExitState() {
        enableRotation = false;
        controller.navMeshAgent.updateRotation = true;
        if (isLookingAround) {
            StopCoroutine(lookAroundCoroutine);
        }
        controller.updateDetectInfo(1);
    }

    private IEnumerator LookAround() {
        isLookingAround = true;
        yield return new WaitForSeconds(2f);
        controller.navMeshAgent.updateRotation = false;
        Vector3 behind = new Vector3(-controller.transform.forward.x, controller.transform.forward.y, -controller.transform.forward.z);
        rotationObjective = Quaternion.LookRotation(behind);
        enableRotation = true;
        yield return new WaitForSeconds(2f);
        enableRotation = false;
        controller.navMeshAgent.updateRotation = false;
        behind = new Vector3(-controller.transform.forward.x, controller.transform.forward.y, -controller.transform.forward.z);
        rotationObjective = Quaternion.LookRotation(behind);
        enableRotation = true;
        controller.updateDetectInfo(1);
        yield return new WaitForSeconds(2f);
        controller.ChangeState(controller.idleState);
    }
}