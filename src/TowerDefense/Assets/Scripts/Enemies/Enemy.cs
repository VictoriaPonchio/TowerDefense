using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float initialSpeed;
    public float currentSpeed;
    [SerializeField]
    private int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;
    public int currencyValue;
    public int playerDamage;

    private Waypoints wp;
    private int wpIndex;

    public event Action<Enemy> OnEnemyDeath;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        wp = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //Moves the character from current position to the waypoint position
        transform.position = Vector2.MoveTowards(transform.position, wp.waypoints[wpIndex].position, currentSpeed * Time.deltaTime);

        //Check the distance between waypoint and enemy
        if (Vector2.Distance(transform.position, wp.waypoints[wpIndex].position) < 0.1f)
        {
            //Increment the waypoint index until the last one
            if (wpIndex < wp.waypoints.Length - 1)
            {
                wpIndex++;
                //Sprite rotation
                Vector3 dir = wp.waypoints[wpIndex].position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                GameManager.Instance.RemoveEnemy();
                var gameObj = transform.gameObject;
                GameObject.Destroy(gameObj);
                player.TakeDamage(this.playerDamage);
            }

        }

    }

    /// <summary>
    /// Set initial values for spawn
    /// </summary>
    public void Spawn()
    {
        wp = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
        transform.position = wp.waypoints[0].position;
        currentSpeed = initialSpeed;
        //initialSpeed = 3;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Decreases the enemy's current health
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Destroys current enemy and increment currency
    /// </summary>
    private void Die()
    {
        OnEnemyDeath?.Invoke(this);
        GameManager.Instance.Currency += this.currencyValue;
        GameManager.Instance.RemoveEnemy();
        Destroy(gameObject);
    }
}
