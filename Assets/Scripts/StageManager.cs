using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public List<GameObject> Stages = new List<GameObject>();
    public List<Transform> StagePos = new List<Transform>();
    private GameObject currentStage;
    public Transform startPos;

    //╫л╠шео
    private static StageManager instance;
    void Awake()
    {
        DOTween.SetTweensCapacity(tweenersCapacity: 500, sequencesCapacity: 50);


        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static StageManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public TMP_Text stageText;
    private int currentStageIndex = 0;

    void Start()
    {
        LoadStage(currentStageIndex);
        UpdateStageText();
    }

    public void ReLoadStage()
    {
        if (currentStage != null)
        {
            Destroy(currentStage);
        }
        LoadStage(currentStageIndex);
    }


    public void AdvanceStage()
    {
        if (currentStage != null)
        {
            Destroy(currentStage);
        }

        currentStageIndex++;
        if (currentStageIndex < Stages.Count)
        {
            LoadStage(currentStageIndex);
        }

        if(currentStageIndex >= 9)
        {
            Debug.Log("all clear");
        }

        UpdateStageText();
    }

    private void LoadStage(int stageIndex)
    {
        currentStage = Instantiate(Stages[stageIndex], StagePos[stageIndex].position, Quaternion.identity);
    }

    private void UpdateStageText()
    {
        if (currentStageIndex <= 8)
        {
            stageText.text = "Stage: " + (currentStageIndex + 1);
        }
        else
        {
            stageText.text = "Stage All Clear! ";
        }
    }
}
