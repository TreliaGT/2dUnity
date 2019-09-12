using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointControl : MonoBehaviour
{
    [SerializeField]
    int Checkboxindex = 0;
    [SerializeField]
    bool checkpointPassed = false;
    [SerializeField]
    CheckPointManager manager;

    public Transform Initialise(int Index, bool pass)
    {
        Checkboxindex = Index;
        checkpointPassed = pass;
        return transform.GetChild(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !checkpointPassed)
        {
            checkpointPassed = true;
            manager.UpdateCheckpoint(Checkboxindex);
        }
    }
}
