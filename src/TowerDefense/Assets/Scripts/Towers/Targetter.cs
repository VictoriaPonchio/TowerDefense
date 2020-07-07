using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetter : MonoBehaviour
{
    CircleCollider2D circleCollider;

    private List<Enemy> targetsInRange = new List<Enemy>();
    private Enemy currentTarget;
    public Enemy CurrentTarget
    {
        get { return currentTarget; }
    }

    public event Action<Enemy> OnTargetAcquired;
    public event Action OnTargetLost;
    public event Action<Enemy> OnTargetInRange;
    public event Action<Enemy> OnTargetOutOfRange;

    public List<Enemy> targetsLst
    {
        get
        {
            return targetsInRange;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if there a target
        if (currentTarget == null)
        {
            currentTarget = GetNearestEnemy();

            if (currentTarget != null && OnTargetAcquired != null)
            {
                OnTargetAcquired(currentTarget);
            }
        }
    }

    /// <summary>
    /// Removes a target from the game
    /// </summary>
    /// <param name="target">Target to be removed</param>
    private void TargetRemoved(Enemy target)
    {
        //Unsubscribe the method from death event
        target.OnEnemyDeath -= TargetRemoved;

        if (currentTarget == target)
        {
            currentTarget = null;
            OnTargetLost?.Invoke();
        }
        targetsInRange.Remove(target);
    }

    /// <summary>
    /// Search for the nearest enemy in range
    /// </summary>
    /// <returns>Target Enemy</returns>
    private Enemy GetNearestEnemy()
    {
        int length = targetsInRange.Count;
        if (length == 0)
        {
            return null;
        }

        //Only get nearest if there are enemies in range
        Enemy nearest = null;
        float distance = float.MaxValue;
        for (int i = length - 1; i >= 0; i--)
        {
            Enemy targetable = targetsInRange[i];
            //If target is not in range anymore
            if (targetable == null)
            {
                targetsInRange.RemoveAt(i);
                continue;
            }
            //Find nearest by distance from enemy (targetable)
            float currentDistance = Vector3.Distance(transform.position, targetable.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                nearest = targetable;
            }
        }
        return nearest;
    }

    #region events

    void OnTriggerEnter2D(Collider2D other)
    {
        var targetable = other.GetComponent<Enemy>();
        if (targetable == null)
        {
            return;
        }

        //Add enemy to list and subscribe the death event
        targetsInRange.Add(targetable);
        targetable.OnEnemyDeath += TargetRemoved;
        OnTargetInRange?.Invoke(targetable);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var targetable = other.GetComponent<Enemy>();
        if (targetable == null)
        {
            return;
        }

        //Remove enemy from list and unsubsribe the death event
        targetsInRange.Remove(targetable);
        TargetRemoved(targetable);
        OnTargetOutOfRange?.Invoke(targetable);
    }

    #endregion
}
