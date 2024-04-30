using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ER_MeleeAttackState : MeleeAttackState
{
    private EnemyRange enemy;

    public ER_MeleeAttackState(Entity entity, BaseStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, EnemyRange enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
            if (isPlayerInMinAgrRange) stateMachine.ChangeState(enemy.playerDetectedState);
            else stateMachine.ChangeState(enemy.lookForPlayerState);
    
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}