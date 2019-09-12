using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCheckpoints : MonoBehaviour
{
    public CheckPointManager manager;
    List<Transform> CheckpointList = new List<Transform>();


     void Awake()
    {
        for (int i = 0; i > transform.childCount; i++)
        {
            Transform currentpoint = transform.GetChild(i);
            bool passedcheckpoint = (i <= manager.CurrentCheckPoint) ? true : false;
            CheckpointList.Add(currentpoint.GetComponent<CheckpointControl>().Initialise(i, passedcheckpoint));
        }

        manager.Initialise(CheckpointList);
    }
}
