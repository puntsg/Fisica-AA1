using System.Collections.Generic;
using UnityEngine;

public static class FK
{
    public static void ApplyFK
    (
        List<Transform> joints,
        Vector3 axis,
        float angle,
        int joint = 0
    )
    {
        Vector3 pivot = joints[joint].position;
        for (int i = joint + 1; i < joints.Count; i++)
        {
            Vector3 pseudoLink = joints[i].position-pivot;
            Vector3 newPseudoLunk = Rotator.QuaternionRot(pseudoLink, angle, axis);
            joints[i].position = pivot + newPseudoLunk;
        }
    }
};
