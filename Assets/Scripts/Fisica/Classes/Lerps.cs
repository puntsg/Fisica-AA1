using System;
using System.Collections;
using UnityEngine;

public static class CustomLerps
{
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return (((1 - t) * a) + (t * b));
    }
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return (((1 - t) * a) + (t * b));
    }
   public static CustomQuaternion SLERP(CustomQuaternion q0, CustomQuaternion q1, float t)
    {

        float dot = q0.w * q1.w + q0.x * q1.x + q0.y * q1.y + q0.z * q1.z;

        if (dot < 0f)
        {
            q1 = CustomQuaternion.Multiply(q1, -1f);
            dot = -dot;
        }

        if (dot > 0.9995f)
        {
            CustomQuaternion lin = CustomQuaternion.Sum(
                CustomQuaternion.Multiply(q0, 1f - t),
                CustomQuaternion.Multiply(q1, t));
            return lin.Normalize();
        }

        float omega = Mathf.Acos(dot);//CustomQuaternion.Angle(q0, q1);
        CustomQuaternion result = CustomQuaternion.Sum(
            CustomQuaternion.Multiply(q0, (MathF.Sin((1-t)*omega)/ MathF.Sin(omega))),
            CustomQuaternion.Multiply(q1, (MathF.Sin(omega*t)/MathF.Sin(omega)))
         ); 
        return result.Normalize();
    }
}