using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPool : MonoBehaviour
{
    public static StepPool Instance;
    public GameObject stepPrefab;
    public int poolSize = 20;

    private List<GameObject> pool;

    void Awake()
    {
        Instance = this;
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(stepPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetStep()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(stepPrefab);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnStep(GameObject obj)
    {
        obj.SetActive(false);
    }
}