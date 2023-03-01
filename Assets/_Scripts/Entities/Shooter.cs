using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used for the Shooter entities
/// </summary>
public class Shooter : Entity // Extend the class Entity
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;

    private int _numberOfBullets;


    private void Start()
    {
        _numberOfBullets = damagePoints;
    }

    public override void BeginPhase()
    {
        base.BeginPhase();
        StartCoroutine(ShootRoutine());
    }

    protected override void EndPhase()
    {
        base.EndPhase();
    }

    /// <summary>
    /// Shooting routing that generate bullets
    /// </summary>
    /// <returns>IEnumerator Time of Fire Rate</returns>
    private IEnumerator ShootRoutine()
    { 
        for(int i = 0; i < _numberOfBullets; i++)
        {
            yield return new WaitForSeconds(fireRate);
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }

        EndPhase();
    }
}
