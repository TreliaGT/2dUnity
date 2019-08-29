using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform Target;
    public float setZ = -10f;

    public float MaxX = 10f;
    public float minX = -10f;

    public float MaxY = 10f;
    public float minY = -10f;

    Vector3 FinalPos;

    // Update is called once per frame
    void LateUpdate()
    {
        if(Target.position.x < MaxX && Target.position.x > minX)
        {
            FinalPos.x = Target.position.x;
        }

        if (Target.position.y < MaxY && Target.position.y > minY)
        {
            FinalPos.y = Target.position.y;
        }
        FinalPos.z = setZ;
        transform.position = FinalPos;
    }
}
