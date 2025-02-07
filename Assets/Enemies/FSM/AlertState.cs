using System.Collections;
using UnityEngine;

public class AlertState : State<EnemyController> {
    
    private Coroutine alertCoroutine;
    private bool coroutineRunning;
    
    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);
        
        controller.navMeshAgent.updateRotation = false;
        controller.navMeshAgent.SetDestination(controller.transform.position);
        alertCoroutine = StartCoroutine(AlertCoroutine());
        
        controller.updateDetectInfo(2);
    }

    public override void OnUpdateState() {
        Quaternion targetRotation = Quaternion.LookRotation(controller.getDirectionToPlayer().normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

        if (!controller.DetectPlayer()) {
            if (coroutineRunning) {
                StopCoroutine(alertCoroutine);
                controller.ChangeState(controller.investigateState);
            }
        }
    }

    public override void OnExitState() {
        controller.navMeshAgent.updateRotation = true;
    }

    private IEnumerator AlertCoroutine() {
        coroutineRunning = true;
        yield return new WaitForSecondsRealtime(1.5f);
        controller.ChangeState(controller.chaseState);
    }
}