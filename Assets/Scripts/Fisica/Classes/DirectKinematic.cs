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
    public static void ApplyFK
    (
        List<Transform> joints,
        Vector3 axis,
        float angle,
        bool transformRotate,
        int joint = 0
        
    )
    {
        if (!transformRotate)
            ApplyFK(joints, axis, angle, joint);
        else
        {
            axis.Normalize();
            //Como en los ejemplos de clase, con el seno y coseno vemos la rotación
            float c = Mathf.Cos(angle * 0.5f);
            float s = Mathf.Sin(angle * 0.5f);

            CustomQuaternion rotation = new CustomQuaternion(c, axis.x * s, axis.y * s, axis.z * s);

            Vector3 pivot = joints[joint].position;
            for (int i = joint; i < joints.Count; i++)
            {
                if (i > joint)
                {
                    Vector3 pseudoLink = joints[i].position - pivot;
                    Vector3 newPseudoLunk = Rotator.QuaternionRot(pseudoLink, rotation);
                    joints[i].position = pivot + newPseudoLunk;
                }
                Quaternion current = joints[i].rotation;
                CustomQuaternion currentRotation = new CustomQuaternion(current.w, current.x, current.y, current.z);
                CustomQuaternion result = CustomQuaternion.Multiply(rotation, currentRotation);
                joints[i].rotation = new Quaternion(result.x, result.y, result.z, result.w);
            }
        }
    }

    public static void ApplyFK
    (
        List<Transform> joints,
        CustomQuaternion rotation,
        int joint = 0
    )
    {
        Vector3 pivot = joints[joint].position;
        for (int i = joint + 1; i < joints.Count; i++)
        {
            Vector3 pseudoLink = joints[i].position - pivot;
            Vector3 newPseudoLunk = Rotator.QuaternionRot(pseudoLink,rotation);
            joints[i].position = pivot + newPseudoLunk;
        }
    }
};
