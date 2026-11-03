using System;
using Unity.VisualScripting;
using UnityEngine;

public class CustomQuaternion
{
    public float w, x, y, z;
    public CustomQuaternion(float new_w, float new_x, float new_y, float new_z)
    {   
        w = new_w;
        x = new_x;
        y = new_y;
        z = new_z;
    }
    public CustomQuaternion(Vector3 p)
    {
        w = 0;
        x = p.x;
        y = p.y;
        z = p.z;
    }
    public float GetQuatNorm()
    {
        return (float)Mathf.Sqrt(w * w + x * x + y * y + z * z);
        //        return ((w * w) + (x * x) + (y * y) + (z * z));
    }
    public CustomQuaternion GetConjugatedQuaternion()
    {
        return new CustomQuaternion(w, -x, -y, -z);
    }
    public CustomQuaternion Normalize()
    {
        float norm = GetQuatNorm();
        if (norm < 1e-8f)
            throw new InvalidOperationException("Cannot normalize a zero quaternion");
        return new CustomQuaternion(
            w / norm,
            x / norm,
            y / norm,
            z / norm
         );
    }
    public CustomQuaternion GetInversedQuaternion()
    {
        float norm = GetQuatNorm();
        return new CustomQuaternion(
            w/norm, 
            x/norm, 
            y/norm,
            z/norm
         );
    }
    public static CustomQuaternion Multiply(CustomQuaternion a, CustomQuaternion b)
    {
        return new CustomQuaternion(
            a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z,
            a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
            a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
            a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w
         );
    }
    public static CustomQuaternion Multiply(CustomQuaternion a, float b)
    {
        return new CustomQuaternion(a.w * b, a.x * b, a.y * b, a.z * b);
    }
    public static CustomQuaternion Sum(CustomQuaternion a, CustomQuaternion b)
    {
        return new CustomQuaternion(a.w + b.w, a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static float Angle(CustomQuaternion a, CustomQuaternion b)
    {
        float p = a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        float absP = Math.Abs(p);
        float clamP = Math.Min(absP, 1.0f);
        float rad = MathF.Acos(clamP) * 2.0f;
        return rad * (180.0f / MathF.PI);
    }
}
