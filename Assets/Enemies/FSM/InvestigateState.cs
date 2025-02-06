using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : State<EnemyController>
{
    public override void OnEnterState(EnemyController controller) {
        base.OnEnterState(controller);
        
        Debug.Log("Investigating...");
    }
    
    public override void OnUpdateState() {
        
    }

    public override void OnExitState() {
        
    }
}
