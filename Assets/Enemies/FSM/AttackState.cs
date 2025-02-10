using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController> {
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCharge = 1.5f;

    private bool coroutineActive;
    private Coroutine coroutine;

    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);
        coroutine = StartCoroutine(AttackCoroutine());
    }

    public override void OnUpdateState() { }

    public override void OnExitState() {
        if (coroutineActive)
        {
            StopCoroutine(coroutine);
        }
        controller.PlayIdleAnimation();
    }

    public IEnumerator AttackCoroutine() {
        coroutineActive = true;
        controller.PlayAttackAnimation();
        yield return new WaitForSeconds(attackCharge);
        if (controller.getDirectionToPlayer().magnitude < attackRange) {
            PlayerHealth playerHealth = controller.getPlayerHealth();
            playerHealth.TakeDamage(15);
        }
        yield return new WaitForSeconds(1.1f);
        coroutineActive = false;
        controller.ChangeState(controller.chaseState);
    }
}