using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private ProjectileTower tower;
    private int towerDamage;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    /// <summary>
    /// Initializes the target, tower and damage
    /// </summary>
    /// <param name="tower"></param>
    public void Initialize(ProjectileTower tower)
    {
        this.target = tower.Targetter.CurrentTarget;
        this.tower = tower;
        this.towerDamage = tower.damage;
    }

    /// <summary>
    /// Moves projectile towards the target
    /// </summary>
    private void FollowTarget()
    {
        if (target == null) {
            Destroy(gameObject);
        }
        if (target != null && target.isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * tower.ProjectileSpeed);

            //Calculate the angle of projectile
            Vector2 dir = target.transform.position - transform.position;
            //Calculate the angle
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //Set the rotation
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    #region events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            target.TakeDamage(towerDamage);
            Destroy(gameObject);
        }
    }
    #endregion
}
