using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : Tower
{
    public float fireRate;
    [SerializeField]
    private int projectileSpeed;
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }
    public int damage;

    private Targetter targetter;
    public Targetter Targetter
    {
        get { return targetter; }
    }

    private Enemy currentTarget;
    private float timeToNextFire;
    Vector3 initalPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(30,0,0,2),
            new TowerUpgrade(50,4,0,0),
            //new TowerUpgrade(2,1,1,2)
        };

        initalPos = transform.position;
        targetter = GetComponent<Targetter>();
        targetter.OnTargetAcquired += TargetAcquired;
        targetter.OnTargetLost += TargetLost;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null)
        {
            LookAt(transform, currentTarget.transform);
            HandleFiring();
        }
    }

    /// <summary>
    /// <see cref="Tower.GetStats"/>
    /// </summary>
    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return $"<color=#4a4a4a><size=20><b>Projectile</b></size></color> {base.GetStats()}" +
                   $"\nRange: {Math.Round(gameObject.transform.localScale.x),2} <color=#2abf2a> +{NextUpgrade.Range} </color> " +
                   $"\nDamage: {damage} <color=#2abf2a> +{NextUpgrade.Damage} </color>" +
                   $"\nFire Rate: {fireRate} <color=#2abf2a> +{NextUpgrade.FireRate} </color>";
        }
        return $"<color=#4a4a4a><size=20><b>Projectile</b></size></color> {base.GetStats()} \nRange: {Math.Round(gameObject.transform.localScale.x),2} \nDamage: {damage} \nFire Rate: {fireRate}";
    }

    /// <summary>
    /// <see cref="Tower.UpgradeTowerStats"/>
    /// </summary>
    public override void UpgradeTowerStats()
    {
        var rangeAtt = gameObject.transform.localScale.x + NextUpgrade.Range;
        damage += NextUpgrade.Damage;
        fireRate += NextUpgrade.FireRate;
        gameObject.transform.localScale = new Vector3(rangeAtt, rangeAtt, rangeAtt);
        base.UpgradeTowerStats();
    }

    /// <summary>
    /// Set current target
    /// </summary>
    /// <param name="target">Target</param>
    private void TargetAcquired(Enemy target)
    {
        currentTarget = target;
    }

    /// <summary>
    /// Removes the current target
    /// </summary>
    private void TargetLost()
    {
        currentTarget = null;
    }

    /// <summary>
    /// Resets thw tower position on target direction
    /// </summary>
    /// <param name="toLook"></param>
    /// <param name="toBeLooked"></param>
    private void LookAt(Transform toLook, Transform toBeLooked)
    {
        toLook.parent.up = toBeLooked.position - toLook.position;
    }

    /// <summary>
    /// Manipulate the fire rate
    /// </summary>
    private void HandleFiring()
    {
        timeToNextFire -= Time.deltaTime;

        if (timeToNextFire <= 0.0f && currentTarget != null)
        {
            Shoot();
            timeToNextFire = 1 / fireRate;
        }
    }

    /// <summary>
    /// Shoots projectile on target
    /// </summary>
    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject("Projectile").GetComponent<Projectile>();
        //Spawn projectile on tower position
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }

    #region events
    private void OnDestroy()
    {
        //Cleanup
        targetter.OnTargetAcquired -= TargetAcquired;
        targetter.OnTargetLost -= TargetLost;
    }
    #endregion

}
