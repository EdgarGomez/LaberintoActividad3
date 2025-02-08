using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController> {
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCharge = 1.5f;

    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);
        StartCoroutine(AttackCoroutine());
    }

    public override void OnUpdateState() { }

    public override void OnExitState() {
        controller.PlayIdleAnimation();
    }

    public IEnumerator AttackCoroutine() {
        controller.PlayAttackAnimation();
        yield return new WaitForSeconds(attackCharge);
        Debug.Log(controller.getDirectionToPlayer().magnitude);
        if (controller.getDirectionToPlayer().magnitude < attackRange) {
            Debug.Log("HIT!!");
            PlayerHealth playerHealth = controller.getPlayerHealth();
            playerHealth.TakeDamage(15);
        } else {
            Debug.Log("NO HIT!!");
        }
        controller.ChangeState(controller.chaseState);
    }
}