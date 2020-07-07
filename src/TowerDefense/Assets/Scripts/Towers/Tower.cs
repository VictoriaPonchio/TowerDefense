using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float cost;
    //private float Cost
    //{
    //    get { return cost; }
    //    //protected set;
    //}
    //Only the towers can acess upgrades
    public TowerUpgrade[] Upgrades { get; protected set; }
    /// <summary>
    /// Current tower level
    /// </summary>
    public int Level { get; protected set; }
    public float SellCost { get { return cost / 2; } }
    public TowerUpgrade NextUpgrade
    {
        get
        {
            if (Upgrades.Length > Level - 1)
            {
                //Return the upgrade for the next level
                return Upgrades[Level - 1];
            }
            return null;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void Awake()
    {
        Level = 1;
    }

    /// <summary>
    /// Upgrades the stats of current tower
    /// </summary>
    public virtual void UpgradeTowerStats()
    {
        cost += NextUpgrade.Cost;
        Level += 1;
    }

    /// <summary>
    /// Get stats from current tower with upgrades buffs
    /// </summary>
    /// <returns>Formatted stats information of tower</returns>
    public virtual string GetStats()
    {
        if (NextUpgrade != null)
        {
            return $"\nLevel: {Level}";
        }
        return $"\nLevel: {Level}";
    } 

    /// <summary>
    /// Show the details of selected tower
    /// </summary>
    public void Detail()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    /// <summary>
    /// Destroys the tower
    /// </summary>
    public void Sell()
    {
        //Lógica de venda
        this.GetComponentInParent<Tiles>().isEmpty = true;
        Destroy(this.transform.parent.gameObject);
    }

}
