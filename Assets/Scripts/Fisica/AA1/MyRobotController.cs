using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MyRobotController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] public Vector2 moveInput = Vector2.zero;
    [SerializeField]public Vector2 mouseInput = Vector2.zero;
    [SerializeField] public float mouseWheelInput = 0;
    [SerializeField] public bool mousePressed = false;

    [Header("ObjectReferences")]
    [SerializeField] GameObject body;
    [SerializeField] List<GameObject> armSegments;
    [SerializeField] GameObject shovel;
    [Header("ObjectReferences/Data")]
    [SerializeField] List<Transform> jointsTransform = new List<Transform>();
    [SerializeField] List<Joint> jointData = new List<Joint>();
    [SerializeField] Transform shovelTransform;
    [SerializeField] Joint shovelJoint;
    [SerializeField] Transform bodyTransform;
    [SerializeField] Joint bodyJoint;
    [SerializeField] List<float> angles = new List<float>();

    [Header("Movement")]
    [SerializeField] float currentSpeed = 0;
    [SerializeField] float acceleration = 2;
    [SerializeField] float maxSpeed = 5;
    [Header("Movemet/Turn")]
    [SerializeField] float currentTurnSpeed = 0;
    [SerializeField] float turnAcceleration = 45;
    [SerializeField] float maxTurnSpeed = 90;

    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 2.0f;
    [Header("Rotation/Body")]
    [SerializeField] float bodyRotation = 0;
    [Header("Rotation/Arm")]
    [SerializeField] int selectedArmJoint = 0;
    [SerializeField] float armRotationX = 0;
    [SerializeField] float armRotationZ = 0;
    [Header("Rotation/Shovel")]
    [SerializeField] float shovelRotation = 0;
    [SerializeField] float shovelSpeed = 0.1f;

    [Header("StateVars")]
    [SerializeField] bool bodyRotationEnabled = true;
    [SerializeField] bool shovelRotationEnabled = true;
    [SerializeField] bool armRotationEnabled = true;
    private void Start()
    {
        //Configuramos las listas
        bodyTransform = body.transform;
        jointsTransform.Add(bodyTransform);
        foreach (GameObject segment in armSegments)
            jointsTransform.Add(segment.transform);
        shovelTransform = shovel.transform;
        jointsTransform.Add(shovelTransform);
        
        bodyJoint = bodyTransform.GetComponent<Joint>();
        foreach(Transform joint in jointsTransform){
            jointData.Add(joint.GetComponent<Joint>());
            angles.Add(0);
        }
        shovelJoint = shovelTransform.GetComponent<Joint>();


        //Esto es para que el retón no escape de la pantalla, ni caso
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void CheckInputs()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseWheelInput = Input.mouseScrollDelta.y;
        mousePressed = Input.GetMouseButton(0);
    }
    private void Update()
    {
        CheckInputs();
        Move();
        //Rodrigo, en un inicio nos estabamos ayudando de esta web https://www.alanzucconi.com/2017/04/06/forward-kinematics/ , pero no cuadraba con lo del FK
        //P = punto
        //D = distancia
        //a = angulo
        //Para el primero --> P1 = P0 + D1
        //La formula queda en, Pi = Pi-1 + rotacion(Di, Pi-1, a0)
        //donde a0 va la acumulación de rotaciones anteriores
        //El for ya viene dentro del applyFK
        if(!mousePressed && bodyRotationEnabled)
            MoveBody();
        if (mousePressed && armRotationEnabled)
            MoveArm();
        if (mouseWheelInput != 0 && shovelRotationEnabled)
            MoveShovel();
        //if(Input.GetKeyDown(KeyCode.R) && (shovelRotationEnabled && armRotationEnabled && bodyRotationEnabled))
        //    RestoreRotations();
    }
    private void Move()
    {
        //Movimiento to' suave
        if(moveInput.y != 0)
            currentSpeed += -moveInput.y * acceleration * Time.deltaTime;
        else
        {
            if (currentSpeed > 0)
                currentSpeed = Mathf.Clamp(currentSpeed - (acceleration*2) * Time.deltaTime, 0, maxSpeed);
            else if(currentSpeed < 0)
                currentSpeed = Mathf.Clamp(currentSpeed + (acceleration * 2) * Time.deltaTime, -maxSpeed, 0);
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        //Rotación to' suave chaval
        if (moveInput.x != 0)
            currentTurnSpeed += moveInput.x * turnAcceleration * Time.deltaTime;
        else
        {
            if (currentTurnSpeed > 0)
                currentTurnSpeed = Mathf.Clamp(currentTurnSpeed - (turnAcceleration * 4) * Time.deltaTime, 0, maxTurnSpeed);
            else if (currentTurnSpeed < 0)
                currentTurnSpeed = Mathf.Clamp(currentTurnSpeed + (turnAcceleration * 4) * Time.deltaTime, -maxTurnSpeed, 0);
        }
        currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, -maxTurnSpeed, maxTurnSpeed);
        transform.Rotate(Vector3.up * currentTurnSpeed * Time.deltaTime);
    }
    void RestoreRotations()
    {
        armRotationEnabled = false;
        LeanTween.value(armRotationX,0,2).setOnUpdate(delegate(float r){
            float delta = r - armRotationX;
            for (int i = 0; i < armSegments.Count; i++)
                FK.ApplyFK(jointsTransform, bodyTransform.right, delta, true, i+1);
            armRotationX = r;

        }).setOnComplete(delegate (){
            armRotationEnabled=true;
        });

        LeanTween.value(armRotationZ, 0, 2).setOnUpdate(delegate (float r) {
            float delta = r - armRotationZ ;
            for (int i = 0; i < armSegments.Count; i++)
                FK.ApplyFK(jointsTransform, bodyTransform.forward, delta, true, i+1);
            armRotationZ = r;
        });

        shovelRotationEnabled = false;
        LeanTween.value(shovelRotation, 0, 2).setOnUpdate(delegate (float r) {
            float delta = r - shovelRotation;
            FK.ApplyFK(jointsTransform, jointsTransform[jointsTransform.Count - 1].right, delta, true, jointsTransform.Count - 1);
            shovelRotation = r;

        }).setOnComplete(delegate () {
            shovelRotationEnabled = true;
        });

        bodyRotationEnabled = false;
        LeanTween.value(bodyRotation, 0, 2).setOnUpdate(delegate (float r) {
            float delta = r - bodyRotation;
            FK.ApplyFK(jointsTransform, transform.up, delta, true, 0);
            bodyRotation = r;

        }).setOnComplete(delegate () {
            bodyRotationEnabled = true;
        });
    }
    void MoveArm()
    {
        Joint baseJoint = jointData[1];
        float newArmAngleX = armRotationX + mouseInput.y * rotationSpeed;
        newArmAngleX = Mathf.Clamp(newArmAngleX, baseJoint.MinAngle, baseJoint.MaxAngle);
        float deltaX = newArmAngleX - armRotationX;
        float newArmAngleZ = armRotationZ + mouseInput.x * rotationSpeed;
        newArmAngleZ = Mathf.Clamp(newArmAngleZ, baseJoint.MinAngle, baseJoint.MaxAngle);
        float deltaZ = newArmAngleZ - armRotationZ;
        for (int i = 0; i < armSegments.Count; i++)
        {
            FK.ApplyFK(jointsTransform, bodyTransform.right, deltaX, true, i+1);
            FK.ApplyFK(jointsTransform, bodyTransform.forward, deltaZ, true, i+1);
        }
        armRotationX = newArmAngleX;
        armRotationZ = newArmAngleZ;
    }
    void MoveShovel()
    {
        float newShovel = shovelRotation + mouseWheelInput * shovelSpeed;
        newShovel = Mathf.Clamp(newShovel, shovelJoint.MinAngle, shovelJoint.MaxAngle);
        float delta = newShovel - shovelRotation;
        FK.ApplyFK(jointsTransform, jointsTransform[jointsTransform.Count-1].right, delta, true, jointsTransform.Count - 1);
        shovelRotation = newShovel;
    }
    void MoveBody()
    {
        float newBody = bodyRotation + mouseInput.x * rotationSpeed;
        newBody %= (2 * Mathf.PI);
        float delta = newBody - bodyRotation;
        FK.ApplyFK(jointsTransform, transform.up, delta, true, 0);
        bodyRotation = newBody;
    }
}
