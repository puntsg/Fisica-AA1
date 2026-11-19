using UnityEngine;

public class CCD : MonoBehaviour
{

    public Transform Joint0;
    public Transform Joint1;
    public Transform Joint2;
    public Transform endEffector;

    private int index = 2;
    private Vector3[] Joints = new Vector3[3];
    private Vector3[] Links = new Vector3[3];

    public Transform target;

    public float tolerance = 1.0f;
    public float maxIterations = 1e5f;
    private int countIterations = 0;
    private float distance;

    private float angleRotation;
    private Vector3 axisRotation;
    Vector3[] referenceVectors = new Vector3[2];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Joints[0] = Joint0.position;
        Joints[1] = Joint1.position;
        Joints[2] = Joint2.position;

        GetLinks();

        distance = Vector3.Magnitude(Links[2]);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countIterations < maxIterations && distance > tolerance)
        {
            Vector3 currentJoint = Joints[index];

            GetVectors(currentJoint);
            angleRotation = GetAngleRotation(referenceVectors);
            axisRotation = GetAxisRotation(referenceVectors);
            Quaternion rotation = Quaternion.AngleAxis(angleRotation*180/Mathf.PI, axisRotation);

            UpdatePosition(index, rotation);

            if (index == 0)
                index = 2;
            else
                index--;

            countIterations++;
        }
        
    }

    void GetLinks()
    {
        for(int i = 0; i < 2; i++)
        {
            Links[i] = Joints[i + 1] - Joints[i];
        }
        Links[2] = endEffector.position - Joints[2];
    }

    void GetVectors(Vector3 joint)
    {
        referenceVectors[0] = Vector3.Normalize(endEffector.position - joint);
        referenceVectors[1] = Vector3.Normalize(target.position - joint);

    }
    float GetAngleRotation(Vector3[] vectors)
    {
        float angle;
        angle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(vectors[0], vectors[1]), -1.0f, 1.0f));
        return angle;
    }
    Vector3 GetAxisRotation(Vector3[] vectors)
    {
        Vector3 axis = Vector3.Cross(vectors[0], vectors[1]);
        axis.Normalize();
        return axis;
    }
    void UpdatePosition(int index, Quaternion q)
    {
        if(index < 2)
        {
            for(int i = index; i < 2; i++)
            {
                Joints[i + 1] = Joints[i] + q * Links[i];
            }
        }

        endEffector.position = Joints[2] + q * Links[2];

        Joint0.position = Joints[0];
        Joint0.position = Joints[1];
        Joint0.position = Joints[2];

        GetLinks();
    }
}
