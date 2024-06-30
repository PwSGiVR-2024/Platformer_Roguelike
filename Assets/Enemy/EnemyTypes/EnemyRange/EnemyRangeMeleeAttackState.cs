using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeMeleeAttackState : MeleeAttackState
{
    private EnemyRange enemy;

    public EnemyRangeMeleeAttackState(Entity entity, BaseStateMachine stateMachine, string animBoolName, Transform attackPosition, MeleeAttackStateData stateData, EnemyRange enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
            if (isPlayerInMinAgroRange) stateMachine.ChangeState(enemy.playerDetectedState);
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