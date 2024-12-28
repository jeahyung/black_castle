using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePool : MonoBehaviour
{
    public GameObject linePrefab;
    public int poolSize = 100;
    private List<GameObject> pool;

    void Awake()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(linePrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetLine()
    {
        if (pool == null || pool.Count == 0)
        {
            Debug.LogWarning("Pool is empty or not initialized!");
            return null;
        }

        foreach (var obj in pool)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }

    public void ReturnLine(GameObject line)
    {
        line.SetActive(false);
    }
}
