using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MyRobotController : MonoBehaviour
{
    [Header("Sinusoidal Test")]
    public List<Transform> armPoints;
    public float speed = 1.0f;
    public float armAmplitude = 10;
    public float shovelAmplitude = 10;
    public Vector3 rotationAxis = new Vector3(1.0f, 0.0f, 0.0f);
    public List<float> previousArmRots;
    public float time = 0.0f;
    public float dt = 0.02f;

    private void Start()
    {
        previousArmRots = new List<float>();
        for (int i = 0; i < armPoints.Count; i++)
            previousArmRots.Add(0);
    }
    void Update()
    {
        float lPhase = time * speed;
        float rPhase = lPhase + Mathf.PI;

        List<float> armRots = new List<float>();
        for (int i = 0; i < armPoints.Count; i++) {
            armRots.Add((float)Mathf.Sin(lPhase) * armAmplitude);
            FK.ApplyFK(armPoints, rotationAxis, -previousArmRots[i], i);
            previousArmRots[i] = armRots[i];
        }
        time += dt;
    }
}
