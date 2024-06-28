using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ER_RangedAttackState : RangedAttackState
{
    private EnemyRange enemy;

    public ER_RangedAttackState(Entity entity, BaseStateMachine stateMachine, string animBoolName, Transform attackPosition, RangedAttackStateData stateData, EnemyRange enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
        if (entity.aliveGameObject.transform.position.x < entity.playerTransform.position.x)
        {
            if (entity.facingDirection != 1) entity.Flip();
        }
        else if (entity.aliveGameObject.transform.position.x > entity.playerTransform.position.x)
        {
            if (entity.facingDirection == 1) entity.Flip();
        }

        if (isAnimationFinished)
            if (isPlayerInMinAgroRange) stateMachine.ChangeState(enemy.playerDetectedState);
            else stateMachine.ChangeState(enemy.lookForPlayerState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}