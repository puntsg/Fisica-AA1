using System.Collections.Generic;
using UnityEngine;

public static class CCDRotation
{
    public static void ApplyCDDRotation(List<Transform> joints, int currentJointIndex, Transform target)
    {
        //for pendiente de revisión
        for (int i = 0; i < joints.Count; i++)
            joints[i].transform.rotation = Quaternion.Euler(CalculateCCDRotation(joints, i, target));
    }
    public static void ApplyBouncyCDDRotation(List<Transform> joints, int currentJointIndex, Transform target)
    {
        //for pendiente de revisión
        for (int i = 0; i < joints.Count; i++)
            for(int j = i; j > 0; j--)
                joints[i].transform.rotation = Quaternion.Euler(CalculateCCDRotation(joints, j, target));
    }
    static Vector3 CalculateCCDRotation(List<Transform> joints,int currentJointIndex, Transform target)
    {
        Vector3 r1 = joints[joints.Count - 1].position - joints[currentJointIndex].position;
        float normaR1 = Mathf.Sqrt((r1.x * r1.x) + (r1.y * r1.y) + (r1.z * r1.z));
        r1 = new Vector3(
            r1.x / normaR1,
            r1.y / normaR1,
            r1.z / normaR1
        );


        Vector3 r2 = target.position - joints[currentJointIndex].position;
        float normaR2 = Mathf.Sqrt((r2.x * r2.x) + (r2.y * r2.y) + (r2.z * r2.z));
        r2 = new Vector3(
            r2.x / normaR2,
            r2.y / normaR2,
            r2.z / normaR2
         );


         Vector3 u1 = new Vector3(r1.x * r2.x, r1.y * r2.y, r1.z * r2.z);
        float rotationAngle = Mathf.Acos(Mathf.Sqrt((u1.x * u1.x) + (u1.y * u1.y) + (u1.z * u1.z)));
        //como r1 y r2 son unitarios, no se divide

         return u1 ;
    }
}
