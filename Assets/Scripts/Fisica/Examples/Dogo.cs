using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Dogo : MonoBehaviour
{
    public Transform head, torso, hip;
    public List<Transform> flLeg, frLeg, blLeg, brLeg, tail;
    private List<List<Transform>> chains = new List<List<Transform>>();
    private List<List<LineRenderer>> bones = new List<List<LineRenderer>>();

    public float walkSpeed;

    public float hipAmplitude;
    public float kneeAmplitude;
    public float ankleAmplitude;

    public float tailAmplitude;

    private Vector3 rotationAxis = new Vector3(0.0f, 0.0f, 1.0f);

    private float previousFLHip = 0.0f;
    private float previousFLKnee = 0.0f;
    private float previousFLAnkle = 0.0f;

    private float previousFRHip = 0.0f;
    private float previousFRKnee = 0.0f;
    private float previousFRAnkle = 0.0f;

    private float previousBLHip = 0.0f;
    private float previousBLKnee = 0.0f;
    private float previousBLAnkle = 0.0f;

    private float previousBRHip = 0.0f;
    private float previousBRKnee = 0.0f;
    private float previousBRAnkle = 0.0f;

    private float previoustail = 0.0f;
    private float previousTailTip = 0.0f;

    private float time = 0.0f;
    public float dt = 0.02f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chains.Add(flLeg);
        chains.Add(frLeg);
        chains.Add(blLeg);
        chains.Add(brLeg);
        chains.Add(tail);
        chains.Add(new List<Transform> { torso, tail[0] });
        chains.Add(new List<Transform> { head, torso });
        chains.Add(new List<Transform> { torso, hip });
        chains.Add(new List<Transform> { torso, flLeg[0] });
        chains.Add(new List<Transform> { torso, frLeg[0] });
        chains.Add(new List<Transform> { torso, blLeg[0] });
        chains.Add(new List<Transform> { torso, brLeg[0] });

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

        float flHipRotation = (float)Mathf.Sin(lPhase) * hipAmplitude;
        float flKneeRotation = (float)Mathf.Sin(-(lPhase + Mathf.PI / 4)) * kneeAmplitude;
        float flAnkleRotation = (float)Mathf.Sin(-(lPhase + Mathf.PI / 2)) * ankleAmplitude;

        float frHipRotation = (float)Mathf.Sin(rPhase) * hipAmplitude;
        float frKneeRotation = (float)Mathf.Sin(-(rPhase + Mathf.PI / 4)) * kneeAmplitude;
        float frAnkleRotation = (float)Mathf.Sin(-(rPhase + Mathf.PI / 2)) * ankleAmplitude;

        float blHipRotation = (float)Mathf.Sin(rPhase) * hipAmplitude;
        float blKneeRotation = (float)Mathf.Sin(-(rPhase + Mathf.PI / 4)) * kneeAmplitude;
        float blAnkleRotation = (float)Mathf.Sin(-(rPhase + Mathf.PI / 2)) * ankleAmplitude;

        float brHipRotation = (float)Mathf.Sin(lPhase) * hipAmplitude;
        float brKneeRotation = (float)Mathf.Sin(-(lPhase + Mathf.PI / 4)) * kneeAmplitude;
        float brAnkleRotation = (float)Mathf.Sin(-(lPhase + Mathf.PI / 2)) * ankleAmplitude;

        float tailRotation = (float)Mathf.Sin(lPhase * 2) * tailAmplitude;
        float tailTipRotation = (float)Mathf.Sin(lPhase * 2 - Mathf.PI / 2) * tailAmplitude;


        FK.ApplyFK(blLeg, rotationAxis, blHipRotation - previousBLHip, 0);
        FK.ApplyFK(blLeg, rotationAxis, blKneeRotation - previousBLKnee, 1);
        FK.ApplyFK(blLeg, rotationAxis, blAnkleRotation - previousBLAnkle, 2);
        FK.ApplyFK(brLeg, rotationAxis, brHipRotation - previousBRHip, 0);
        FK.ApplyFK(brLeg, rotationAxis, brKneeRotation - previousBRKnee, 1);
        FK.ApplyFK(brLeg, rotationAxis, brAnkleRotation - previousBRAnkle, 2);

        FK.ApplyFK(flLeg, rotationAxis, flHipRotation - previousFLHip, 0);
        FK.ApplyFK(flLeg, rotationAxis, flKneeRotation - previousFLKnee, 1);
        FK.ApplyFK(flLeg, rotationAxis, flAnkleRotation - previousFLAnkle, 2);
        FK.ApplyFK(frLeg, rotationAxis, frHipRotation - previousFRHip, 0);
        FK.ApplyFK(frLeg, rotationAxis, frKneeRotation - previousFRKnee, 1);
        FK.ApplyFK(frLeg, rotationAxis, frAnkleRotation - previousFRAnkle, 2);

        FK.ApplyFK(tail, rotationAxis, tailRotation - previoustail, 0);
        FK.ApplyFK(tail, rotationAxis, tailTipRotation - previousTailTip, 1);


        previousBLHip = blHipRotation;
        previousBLKnee = blKneeRotation;
        previousBLAnkle = blAnkleRotation;

        previousBRHip = brHipRotation;
        previousBRKnee = brKneeRotation;
        previousBRAnkle = brAnkleRotation;

        previousFLHip = flHipRotation;
        previousFLKnee = flKneeRotation;
        previousFLAnkle = flAnkleRotation;

        previousFRHip = frHipRotation;
        previousFRKnee = frKneeRotation;
        previousFRAnkle = frAnkleRotation;

        previoustail = tailRotation;
        previousTailTip = tailTipRotation;

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