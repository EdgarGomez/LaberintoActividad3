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

    public override void OnExitState() { }

    public IEnumerator AttackCoroutine() {
        yield return new WaitForSeconds(attackCharge);
        if (controller.getDirectionToPlayer().magnitude < attackRange) {
            PlayerHealth playerHealth = controller.getPlayerHealth();
            playerHealth.TakeDamage(15);
        }
        controller.ChangeState(controller.chaseState);
    }
}