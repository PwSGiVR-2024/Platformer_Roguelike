using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newEntityData",menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float maxHealth = 30f;

    public float damageHopSpeed = 10f;

    public float wallCheckDistance = 0.2f;
    public float LedgeCheckDistance= 0.4f;
    public float groundCheckRadius = 0.3f;

    public float minAgroDistance = 7f;
    public float maxAgroDistance = 8f;

    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;
    public float closeRangeActionDistance = 4f;

    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlayer;
}
