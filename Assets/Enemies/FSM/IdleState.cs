using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<EnemyController> {
    private Coroutine waitRoutine;

    private bool coroutineComplete;

    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);
        
        coroutineComplete = false;
        waitRoutine = StartCoroutine(waitToContinue());
        
        controller.updateDetectInfo(0);
    }

    public override void OnExitState() {
        if (waitRoutine != null && !coroutineComplete) {
            StopCoroutine(waitRoutine);
        }
        controller.patrolState.updatePoint();
    }

    public override void OnUpdateState() {
        if (controller.DetectPlayer()) {
            controller.ChangeState(controller.lookingState);
        } else {
            controller.decreaseAlertTimer();
        }
    }

    public IEnumerator waitToContinue() {
        yield return new WaitForSeconds(2f);
        coroutineComplete = true;
        controller.ChangeState(controller.patrolState);
    }
}