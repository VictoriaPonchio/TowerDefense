using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private Sprite sprite;
    private Vector3 rangeSize;
    private float cost;
    public float Cost
    {
        get
        {
            return cost;
        }
    }
    public Vector3 RangeSize
    {
        get
        {
            return rangeSize;
        }
    }
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }
    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    void Start()
    {
        this.rangeSize = towerPrefab.transform.GetChild(0).transform.localScale;
        this.cost = GetCost(towerPrefab.transform.tag);
    }

    /// <summary>
    /// Get current tower cost
    /// </summary>
    /// <param name="type"></param>
    /// <returns>Tower cost</returns>
    public float GetCost(string type)
    {
        float cost = 0;

        switch (type)
        {
            case "ProjectileTower":
                ProjectileTower projectile = towerPrefab.GetComponentInChildren<ProjectileTower>();
                cost = projectile.cost;
                break;
            case "DebuffTower":
                DebuffTower debuff = towerPrefab.GetComponentInChildren<DebuffTower>();
                cost = debuff.cost;
                break;
            case "AoeTower":
                AoeTower aoe = towerPrefab.GetComponentInChildren<AoeTower>();
                cost = aoe.cost;
                break;
            default:
                break;
        }
        return cost;
    }

    /// <summary>
    /// Show stats info from current tower
    /// </summary>
    /// <param name="type"></param>
    public void ShowInfo(string type)
    {
        GameManager.Instance.SetTowerTooltip(GetTooltipByType(type));
        GameManager.Instance.ShowStats();
    }

    /// <summary>
    /// Get tooltip from current tower 
    /// </summary>
    /// <param name="type">Tower type</param>
    /// <returns>Formated tooltip</returns>
    private string GetTooltipByType(string type)
    {
        string tooltip = string.Empty;

        switch (type)
        {
            case "ProjectileTower":
                ProjectileTower projectile = towerPrefab.GetComponentInChildren<ProjectileTower>();
                tooltip = $"<color=#4a4a4a><size=20><b>Projectile</b></size></color> \nDamage: {projectile.damage} \nFireRate: {projectile.fireRate} \nFires projectiles at enemies in range";
                break;
            case "DebuffTower":
                DebuffTower debuff = towerPrefab.GetComponentInChildren<DebuffTower>();
                tooltip = string.Format($"<color=#00ffffff><size=20><b>Ice</b></size></color>\nSlow Factor: {debuff.slowFactor * 100}%\nSlows down targets");
                break;
            case "AoeTower":
                AoeTower aoe = towerPrefab.GetComponentInChildren<AoeTower>();
                tooltip = string.Format($"<color=#ff7449><size=20><b>Fire</b></size></color>\nDamage: {aoe.damage} \nDamages every enemy every {aoe.fireRate} seconds");
                break;
            default:
                break;
        }
        return tooltip;
    }

}
