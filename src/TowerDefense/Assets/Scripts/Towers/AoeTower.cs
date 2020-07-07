using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeTower : Tower
{
    public int damage;
    public int fireRate;
    private float nextActionTime = 0;
    Targetter targetter;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(50,2,0,2),
            new TowerUpgrade(80,4,1,0)
            //new TowerUpgrade(2,1,1,1)
        };

        targetter = GetComponent<Targetter>();
    }

    // Update is called once per frame
    void Update()
    {
        nextActionTime -= Time.deltaTime;

        //Damage each period of fireRate
        if (nextActionTime <= 0.0f)
        {
            nextActionTime = 1 / fireRate;
            AreaDamage();
        }
    }

    /// <summary>
    /// <see cref="Tower.GetStats"/>
    /// </summary>
    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return $"<color=#ff7449><size=20><b>Fire</b></size></color>, {base.GetStats()} " +
                   $"\nRange: {Math.Round(gameObject.transform.localScale.x),2} <color=#2abf2a> +{NextUpgrade.Range} </color>" +
                   $"\nDamage: {damage} <color=#2abf2a> +{NextUpgrade.Damage} </color>" +
                   $"\nFire Rate: {fireRate} <color=#2abf2a> +{NextUpgrade.FireRate} </color>";
        }
        return $"<color=#ff7449><size=20><b>Fire</b></size></color> {base.GetStats()} \nRange: {Math.Round(gameObject.transform.localScale.x),2} \nDamage: {damage} \nFire Rate: {fireRate}";
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
    /// Damages all targets in tower range
    /// </summary>
    private void AreaDamage()
    {
        var enemiesOnTarget = targetter.targetsLst;

        for (int i = 0; i < enemiesOnTarget.Count; i++)
        {
            if (enemiesOnTarget[i].isActiveAndEnabled)
                enemiesOnTarget[i].TakeDamage(damage);
        }
    }
}
