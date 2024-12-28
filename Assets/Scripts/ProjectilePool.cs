using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;
    public GameObject projectilePrefab;
    public int poolSize = 100;

    private List<GameObject> pool;

    void Awake()
    {
        Instance = this;
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetProjectile()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(projectilePrefab);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnProjectile(GameObject obj)
    {
        obj.SetActive(false);
    }
}
