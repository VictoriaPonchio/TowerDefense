using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the towers upgrades
/// </summary>
public class TowerUpgrade
{
    public int Cost { get; private set; }
    public int Damage { get; private set; }
    public int FireRate { get; private set; }
    public int Range { get; private set; }
    public float SlowFactor { get; private set; }

    /// <summary>
    /// Updates tower stats 
    /// </summary>
    /// <param name="cost">Upgrade cost</param>
    /// <param name="damage">New damage</param>
    /// <param name="fireRate">New fire rate</param>
    /// <param name="range">New range</param>
    public TowerUpgrade(int cost, int damage, int fireRate, int range)
    {
        this.Cost = cost;
        this.Damage = damage;
        this.FireRate = fireRate;
        this.Range = range;
    }

    /// <summary>
    /// Updates tower stats 
    /// </summary>
    /// <param name="cost">Upgrade cost</param>
    /// <param name="range">New range</param>
    /// <param name="slowFactor">New slow factor</param>
    public TowerUpgrade(int cost, int range, float slowFactor)
    {
        this.Cost = cost;
        this.Range = range;
        this.SlowFactor = slowFactor;
    }


}
