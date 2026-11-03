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
        float omega = CustomQuaternion.Angle(q0, q1);
        CustomQuaternion result = CustomQuaternion.Sum(
            CustomQuaternion.Multiply(q0, (MathF.Sin((1-t)*omega)/ MathF.Sin(omega))),
            CustomQuaternion.Multiply(q1, (MathF.Sin(omega*t)/MathF.Sin(omega)))
         ); 
        return result;
    }
}