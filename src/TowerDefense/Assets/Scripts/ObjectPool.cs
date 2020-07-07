using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //monsters prefabs 
    [SerializeField]
    private GameObject[] objectPrefabs;

    /// <summary>
    /// Instantiate object of a defined type in the pool of objects
    /// </summary>
    /// <param name="type">Type to be instantiated</param>
    /// <returns>Instantiated gameObject</returns>
    public GameObject GetObject(string type)
    {
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name == type)
            {
                GameObject newObject = Instantiate(objectPrefabs[i], this.transform);
                newObject.name = type;
                return newObject;
            }
        }
        return null;
    }
}
