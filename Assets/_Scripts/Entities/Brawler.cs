using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used for the Brawler Entities
/// </summary>
public class Brawler : Entity // Extend the class Entity
{
    //Using a raycast for damage other Entity in front
    [Header("Raycast Attack Range")]
    public LayerMask attackReceiver;
    public float meleeAttackRange = 1.0f;


    public override void BeginPhase()
    {
        base.BeginPhase();
        MeleeAttack();
    }

    protected override void EndPhase()
    {
        base.EndPhase();
    }

#if DEBUG
    private void Update()
    {
        Debug.DrawRay(transform.position, -transform.up * meleeAttackRange);
    }
#endif

    /// <summary>
    /// Melee Attack using the Raycast in front of this entity
    /// </summary>
    public void MeleeAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, meleeAttackRange, attackReceiver); // Get what it does hit.

        if (hit) // If it hit with something (hit is not null).
        {
            if (hit.transform.TryGetComponent<Entity>(out Entity ent))
            {
                ent.GetDamage(damagePoints);
            }
        }

        EndPhase();
    }
}
