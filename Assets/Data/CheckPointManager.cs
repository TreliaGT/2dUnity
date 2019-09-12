using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class CheckPointManager : ScriptableObject
{
    public int CurrentCheckPoint = 0;
    public List<Transform> newSpawnPoints = new List<Transform>();

    public void Initialise(List<Transform> SpawnPoints)
    {
        SpawnPoints.Clear();
        SpawnPoints = newSpawnPoints;
    }

    public void UpdateCheckpoint(int newCheckpoint)
    {
        CurrentCheckPoint = newCheckpoint;

    }

    public Transform GetCurrentSpawnpoint()
    {
        return newSpawnPoints[CurrentCheckPoint];
    }

    public void resetCheckpoint()
    {
        CurrentCheckPoint = 0;
    }
}
