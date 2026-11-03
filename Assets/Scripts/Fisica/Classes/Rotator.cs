using System.Text.RegularExpressions;
using UnityEngine;

public static class Rotator
{
    public static Vector2 Complex2DRotation(Vector2 p, float angle)
    {
        ComplexNum z = new ComplexNum(p.x,p.y);
        ComplexNum alpha = new ComplexNum(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
        );
        ComplexNum result = ComplexNum.Multiply(alpha,z);
        return new Vector2(result.real, result.img);    
    }
    public static Vector2 Rotate2DVector(Vector2 p, float angle)
    {
        float c = Mathf.Cos(angle);
        float s = Mathf.Sin(angle);

        float new_x = c * p.x - s * p.y;
        float new_y = s * p.x + c * p.y;

        return new Vector2(new_x,new_y);
    }
    public static Vector3 Rotate3DVector(Vector3 p, Vector3 angles)
    {
        float xc = Mathf.Cos(angles.x), xs = Mathf.Sin(angles.x);
        float yc = Mathf.Cos(angles.y), ys = Mathf.Sin(angles.y);
        float zc = Mathf.Cos(angles.z), zs = Mathf.Sin(angles.z);

        float x = p.x, y = p.y, z = p.z;
        float tx, ty, tz;

        tx = zc * x - zs * y;
        ty = zs * x + zc * y;
        x = tx; y = ty;

        ty = xc * y - xs * z;
        tz = xs * y + xc * z;
        y = ty; z = tz;

        tx = yc * x + ys * z;
        tz = -ys * x + yc * z;
        x = tx; z = tz;

        return new Vector3(x, y, z);
    }

    public static Vector3 QuaternionRot(Vector3 p, float angle, Vector3 axis)
    {


        CustomQuaternion q = new CustomQuaternion(0, p.x, p.y, p.z);

        axis.Normalize();

        float c = Mathf.Cos((float)(angle * 0.5));
        float s = Mathf.Sin((float)(angle * 0.5));

        CustomQuaternion alpha = new CustomQuaternion(c, axis.x * s, axis.y * s, axis.z * s);
        CustomQuaternion inverse_alpha = alpha.GetConjugatedQuaternion();

        CustomQuaternion result = CustomQuaternion.Multiply(alpha, CustomQuaternion.Multiply(q, inverse_alpha));

        return new Vector3(result.x, result.y, result.z);


    }


    public static Vector3 QuaternionRot(Vector3 p, CustomQuaternion q)
    {

        q = q.Normalize();
        CustomQuaternion p_quat = new CustomQuaternion(0.0f, p.x, p.y, p.z);
        CustomQuaternion inverse_q = q.GetConjugatedQuaternion();

        CustomQuaternion result = CustomQuaternion.Multiply(q, CustomQuaternion.Multiply(p_quat, inverse_q));

        return new Vector3(result.x, result.y, result.z);
    }

}
