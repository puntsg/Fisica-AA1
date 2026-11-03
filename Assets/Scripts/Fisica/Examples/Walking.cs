using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Walking : MonoBehaviour
{
    public Transform head, torso;
    public List<Transform> lLeg, rLeg, lArm, rArm;
    private List<List<Transform>> chains = new List<List<Transform>>();
    private List<List<LineRenderer>> bones = new List<List<LineRenderer>>();

    public float walkSpeed;

    public float hipAmplitude;
    public float kneeAmplitude;
    public float ankleAmplitude;

    [FormerlySerializedAs("wristAmplitude")]
    public float shoulderAmplitude;
    public float elbowAmplitude;

    private Vector3 rotationAxis = new Vector3(0.0f, 0.0f, 1.0f);

    private float previousLHip = 0.0f;
    private float previousLKnee = 0.0f;
    private float previousLAnkle = 0.0f;

    private float previousRHip = 0.0f;
    private float previousRKnee = 0.0f;
    private float previousRAnkle = 0.0f;

    private float previousLShoulder = 0.0f;
    private float previousLElbow = 0.0f;

    private float previousRShoulder = 0.0f;
    private float previousRElbow = 0.0f;

    private float time = 0.0f;
    public float dt = 0.02f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chains.Add(lLeg);
        chains.Add(rLeg);
        chains.Add(lArm);
        chains.Add(rArm);

        chains.Add(new List<Transform> { head, torso });
        chains.Add(new List<Transform> { torso, lLeg[0] });
        chains.Add(new List<Transform> { torso, rLeg[0] });
        chains.Add(new List<Transform> { torso, lArm[0] });
        chains.Add(new List<Transform> { torso, rArm[0] });

        for (int i = 0; i < chains.Count; i++)
        {
            List<LineRenderer> links = new List<LineRenderer>();
            CreateLinks(links, chains[i]);
            InitializeLineRenderer(links, chains[i]);
            bones.Add(links);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float lPhase = time * walkSpeed;
        float rPhase = lPhase + Mathf.PI; 

        float lHipRotation = (float)Mathf.Sin(lPhase) * hipAmplitude;
        float lKneeRotation = (float)Mathf.Sin(-(lPhase + Mathf.PI / 4)) * kneeAmplitude;
        float lAnkleRotation = (float)Mathf.Sin(-(lPhase + Mathf.PI / 2)) * ankleAmplitude;

        float rHipRotation = (float)Mathf.Sin(rPhase) * hipAmplitude;
        float rKneeRotation = (float)Mathf.Sin(-(rPhase + Mathf.PI / 4)) * kneeAmplitude;
        float rAnkleRotation = (float)Mathf.Sin(-(rPhase + Mathf.PI / 2)) * ankleAmplitude;

        float lShoulderRotation = (float)Mathf.Sin(rPhase) * shoulderAmplitude;
        float lElbowRotation = (float)Mathf.Sin(-(rPhase + Mathf.PI / 4)) * elbowAmplitude;
        float rShoulderRotation = (float)Mathf.Sin(lPhase) * shoulderAmplitude;
        float rElbowRotation = (float)Mathf.Sin(-(lPhase + Mathf.PI / 4)) * elbowAmplitude;


        FK.ApplyFK(lLeg, rotationAxis, lHipRotation - previousLHip, 0);
        FK.ApplyFK(lLeg, rotationAxis, lKneeRotation - previousLKnee, 1);
        FK.ApplyFK(lLeg, rotationAxis, lAnkleRotation - previousLAnkle, 2);
        FK.ApplyFK(rLeg, rotationAxis, rHipRotation - previousRHip, 0);
        FK.ApplyFK(rLeg, rotationAxis, rKneeRotation - previousRKnee, 1);
        FK.ApplyFK(rLeg, rotationAxis, rAnkleRotation - previousRAnkle, 2);

        FK.ApplyFK(lArm, rotationAxis, lShoulderRotation - previousLShoulder, 0);
        FK.ApplyFK(lArm, rotationAxis, lElbowRotation - previousLElbow, 1);
        FK.ApplyFK(rArm, rotationAxis, rShoulderRotation - previousRShoulder, 0);
        FK.ApplyFK(rArm, rotationAxis, rElbowRotation - previousRElbow, 1);


        previousLHip = lHipRotation;
        previousLKnee = lKneeRotation;
        previousLAnkle = lAnkleRotation;

        previousRHip = rHipRotation;
        previousRKnee = rKneeRotation;
        previousRAnkle = rAnkleRotation;

        previousLShoulder = lShoulderRotation;
        previousLElbow = lElbowRotation;

        previousRShoulder = rShoulderRotation;
        previousRElbow = rElbowRotation;

        for (int i = 0; i < chains.Count; i++)
            UpdateVisualLinks(chains[i], bones[i]);

        time += dt;
    }

    void CreateLinks(List<LineRenderer> links, List<Transform> joints)
    {

        links.Clear();
        for (int index = 0; index < joints.Count - 1; index++)
        {
            GameObject obj = new GameObject("Link_" + index);
            LineRenderer line = obj.AddComponent<LineRenderer>();
            links.Add(line);
        }
    }

    void InitializeLineRenderer(
        List<LineRenderer> links,
        List<Transform> joints
        )
    {
        for (int index = 0; index < joints.Count - 1; index++)
        {

            links[index].positionCount = 2;
            links[index].SetPosition(0, joints[index].position);
            links[index].SetPosition(1, joints[index + 1].position);

            links[index].startWidth = 0.1f;
            links[index].endWidth = 0.1f;

            links[index].material = new Material(Shader.Find("Sprites/Default"));

            links[index].startColor = Color.white;
            links[index].endColor = Color.white;

        }
    }

    void UpdateVisualLinks(List<Transform> joints, List<LineRenderer> links)
    {
        for (int index = 0; index < joints.Count - 1; index++)
        {
            links[index].SetPosition(0, joints[index].position);
            links[index].SetPosition(1, joints[index + 1].position);
        }
    }
}