using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FKRotator : MonoBehaviour
{
    public List<Transform> joints;
    public List<LineRenderer> links;
    public float angleRotation = Mathf.PI / 3;
    public Vector3 axis = Vector3.forward;

    void CreateLinks()
    {
        links = new List<LineRenderer>();
        for (int i = 0; i < joints.Count; i++)
        {

            GameObject obj = new GameObject("link" + i);
            LineRenderer lr = obj.AddComponent<LineRenderer>();
            links.Add(lr);
        }
    }
    void InitLineRenderer(List<LineRenderer> links, List<Transform> joints)
    {
        for (int i = 0; i < joints.Count; i++)
        {
            links[i].positionCount = 2;
            links[i].SetPosition(0, joints[i].position);
            links[i].SetPosition(1, joints[i].position);

            links[i].startWidth = .1f;
            links[i].endWidth = .1f;

            links[i].material = new Material(Shader.Find("Sprites/Default"));

            links[i].startColor = Color.white;
            links[i].endColor = Color.white;
        }
    }

    void UpdateLinks()
    {
        for (int i = 0; i < joints.Count; i++)
        {
            links[i].positionCount = 2;
            links[i].SetPosition(0, joints[i].position);
            links[i].SetPosition(1, joints[i].position);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateLinks();
        InitLineRenderer(links, joints);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < 1)
        {
            FK.ApplyFK(joints, axis, angleRotation * Time.deltaTime, joints.Count);
            UpdateLinks();
        }
    }
}
