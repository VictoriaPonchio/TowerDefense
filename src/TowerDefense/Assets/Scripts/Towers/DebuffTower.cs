using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTower : Tower
{
    public float slowFactor;
    Targetter targetter;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(20,1,0.05f),
            new TowerUpgrade(40,1,0.1f)
            //new TowerUpgrade(2,0,0.2f),
        };

        targetter = GetComponent<Targetter>();
        targetter.OnTargetInRange += TargetInRange;
        targetter.OnTargetOutOfRange += TargetOutOfRange;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// <see cref="Tower.GetStats"/>
    /// </summary>
    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return $"<color=#00ffffff><size=20><b>Ice</b></size></color> {base.GetStats()} " +
                $"\nRange: {Math.Round(gameObject.transform.localScale.x),2} <color=#2abf2a> +{NextUpgrade.Range} </color> " +
                $"\nSlow Factor: {slowFactor * 100}% <color=#2abf2a> +{NextUpgrade.SlowFactor * 100}% </color>";
        }
        return $"<color=#00ffffff><size=20><b>Ice</b></size></color> {base.GetStats()} \nRange: {Math.Round(gameObject.transform.localScale.x),2} \nSlow Factor: {slowFactor * 100}%";
    }

    /// <summary>
    /// <see cref="Tower.UpgradeTowerStats"/>
    /// </summary>
    public override void UpgradeTowerStats()
    {
        var rangeAtt = gameObject.transform.localScale.x + NextUpgrade.Range;
        slowFactor += NextUpgrade.SlowFactor;
        gameObject.transform.localScale = new Vector3(rangeAtt, rangeAtt, rangeAtt);
        base.UpgradeTowerStats();
    }

    /// <summary>
    /// Apllies debuff
    /// </summary>
    /// <param name="target">Current target</param>
    private void TargetInRange(Enemy target)
    {
        ApplySlowDebuff(target);
    }

    /// <summary>
    /// Removes debuff
    /// </summary>
    /// <param name="target">Current target</param>
    private void TargetOutOfRange(Enemy target)
    {
        RemoveSlowDebuff(target);
    }

    /// <summary>
    /// Aplies slow debuff on target
    /// </summary>
    /// <param name="target">Target</param>
    private void ApplySlowDebuff(Enemy target)
    {
        var slowDebuff = target.GetComponent<EnemyDebuff>();
        if (slowDebuff == null)
        {
            slowDebuff = target.gameObject.AddComponent<EnemyDebuff>();
        }
        slowDebuff.ApplyDebuff(slowFactor);
    }

    /// <summary>
    /// Removes the slow debuff from target
    /// </summary>
    /// <param name="target"Target></param>
    private void RemoveSlowDebuff(Enemy target)
    {
        var slowDebuff = target.GetComponent<EnemyDebuff>();
        if (slowDebuff == null)
        {
            slowDebuff = target.gameObject.AddComponent<EnemyDebuff>();
        }
        slowDebuff.RemoveDebuff(slowFactor);
    }
    
    #region events
    private void OnDestroy()
    {
        //Cleanup
        targetter.OnTargetInRange -= TargetInRange;
        targetter.OnTargetOutOfRange -= TargetOutOfRange;
    }
    #endregion
}
