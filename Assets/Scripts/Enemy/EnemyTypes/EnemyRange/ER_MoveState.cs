using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ER_MoveState : MoveState
{
    private EnemyRange enemy;
    public ER_MoveState(Entity entity, BaseStateMachine stateMachine, string animBoolName, MoveStateData stateData, EnemyRange enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Debug.Log(isDetectingLedge+"ledge");
        Debug.Log(isDetectingWall + "wall");
        Debug.Log(isEnemyInRange + "enemy");
        if (isPlayerInMinAgroRange) stateMachine.ChangeState(enemy.playerDetectedState);
        else if (isEnemyInRange)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (isDetectingWall || !isDetectingLedge) 
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
