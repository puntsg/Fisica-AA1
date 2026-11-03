using UnityEngine;

public struct ComplexNum
{
    public float real;
    public float img;
    public ComplexNum(float r, float i){real = r; img = i;}
    public static ComplexNum Multiply(ComplexNum a, ComplexNum b)
    {
        float real = a .real * b.real - a.img * b.img;
        float img = a.real * b.img + a.img * b.real;
        return new ComplexNum(real, img);
    }
}
