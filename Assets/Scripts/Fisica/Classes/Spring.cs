using UnityEngine;

public class Spring
{
    public static float Clamp(float val, float min, float max)
    {
        if (val < min) return min;
        if(val > max) return max;
        return val;
    }

    public static void Decompose(CustomQuaternion q, out CustomQuaternion swing, out CustomQuaternion twist)
    {
        q.Normalize();
        float length = Mathf.Sqrt(q.w * q.w + q.z * q.z);
        twist = new CustomQuaternion(q.w / length, 0, 0, q.z / length);
        swing = CustomQuaternion.Multiply(q, twist.GetConjugatedQuaternion());
        swing.Normalize();
    }

    public static Vector2 GetAxisSwing(CustomQuaternion swing)
    {
        float lenght = (float)Mathf.Sqrt(swing.x * swing.x + swing.y * swing.y);
        float angle = 2f * (float)Mathf.Atan2(lenght, swing.w);
        return new Vector2(swing.x * angle/lenght, swing.y* angle/ lenght);
    }
    public static CustomQuaternion FromSwingCoordinates(Vector2 swing)
    {
        float angle = (float)Mathf.Sqrt(swing.x * swing.x + swing.y * swing.y);
        Vector3 axis = new Vector3(swing.x / angle, swing.y / angle, 0);
        return new CustomQuaternion(angle,swing.x,swing.y,0);
    }
    public static float GetTwistedAngle(CustomQuaternion twist)
    {
        return 2f * Mathf.Atan2(twist.z, twist.w);
    }
}
