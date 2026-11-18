using System.Collections.Generic;
using UnityEngine;

public class MyRobotController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField]public Vector2 mouseInput = Vector2.zero;
    [SerializeField] public float mouseWheelInput = 0;
    [SerializeField] public bool mousePressed = false;

    [Header("ObjectReferences")]
    [SerializeField] List<GameObject> armSegments;
    [SerializeField] GameObject shovel;
    [Header("ObjectReferences/Data")]
    [SerializeField] List<Transform> jointsTransform = new List<Transform>();
    [SerializeField] List<Joint> jointData = new List<Joint>();
    [SerializeField] Transform shovelTransform;
    [SerializeField] Joint shovelJoint; 
    [SerializeField] List<float> angles = new List<float>();

    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 2.0f;
    [Header("Rotation/Arm")]
    [SerializeField] int selectedArmJoint = 0;
    [SerializeField] float armRotation = 0;
    [Header("Rotation/Shovel")]
    [SerializeField] float shovelRotation = 0;
    [SerializeField] float shovelSpeed = 0.1f;


    private void Start()
    {
        //Configuramos las listas
        foreach (GameObject segment in armSegments)
            jointsTransform.Add(segment.transform);
        shovelTransform = shovel.transform;
        jointsTransform.Add(shovelTransform);
        
        foreach(Transform joint in jointsTransform){
            jointData.Add(joint.GetComponent<Joint>());
            angles.Add(0);
        }
        shovelJoint = shovelTransform.GetComponent<Joint>();
    }

    void CheckInputs()
    {
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseWheelInput = Input.mouseScrollDelta.y;
        mousePressed = Input.GetMouseButton(0);
    }
    private void Update()
    {
        CheckInputs();
        //Rodrigo, en un inicio nos estabamos ayudando de esta web https://www.alanzucconi.com/2017/04/06/forward-kinematics/ , pero no cuadraba con lo del FK
        //P = punto
        //D = distancia
        //a = angulo
        //Para el primero --> P1 = P0 + D1
        //La formula queda en, Pi = Pi-1 + rotacion(Di, Pi-1, a0)
        //donde a0 va la acumulación de rotaciones anteriores
        //El for ya viene dentro del applyFK

        if (mousePressed)
        {
            Joint baseJoint = jointData[0];
            float newArmAngle = armRotation + mouseInput.y * rotationSpeed ;
            newArmAngle = Mathf.Clamp(newArmAngle, baseJoint.MinAngle, baseJoint.MaxAngle);
            float delta = newArmAngle - armRotation;
            for (int i = 0; i < armSegments.Count; i++)
                FK.ApplyFK(jointsTransform, jointData[i].Axis, delta, true, i);
            armRotation = newArmAngle;
        }
        if (mouseWheelInput != 0)
        {
            float newShovel = shovelRotation + mouseWheelInput * shovelSpeed;
            newShovel = Mathf.Clamp(newShovel, shovelJoint.MinAngle, shovelJoint.MaxAngle);
            float delta = newShovel - shovelRotation;
            FK.ApplyFK(jointsTransform, shovelJoint.Axis, delta, true, jointsTransform.Count - 1);
            shovelRotation = newShovel;
        }
    }
}
