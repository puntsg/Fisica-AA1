using UnityEngine;

public class TestJoint : MonoBehaviour
{
    public Transform baseJoint;
    public Transform ballJoint;
    public Transform endEffector;

    public float targetSwingXDegrees;
    public float targetSwingYDegrees;
    public float targetTwistDegrees;

    public float coneAngleLimit = 60;
    public float minTwist = -30;
    public float maxTwist = 30;

    private CustomQuaternion initialJointPosition;
    private Vector3 link;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialJointPosition = new CustomQuaternion(
            ballJoint.rotation.w, 
            ballJoint.rotation.x, 
            ballJoint.rotation.y, 
            ballJoint.rotation.z
        );
        initialJointPosition.Normalize();
        
        link = endEffector.position - ballJoint.position;

        
    }

    // Update is called once per frame
    void Update()
    {
        float swingX = targetSwingXDegrees * Mathf.PI / 180f;
        float swingY = targetSwingYDegrees * Mathf.PI / 180f;
        float twist = targetTwistDegrees * Mathf.PI / 180f;

        CustomQuaternion requestedSwing = Spring.FromSwingCoordinates(new Vector2(swingX, swingY));

        CustomQuaternion requestedTwist = new CustomQuaternion(twist, new Vector3(0,0,1));

        CustomQuaternion requested = CustomQuaternion.Multiply(requestedTwist, requestedSwing);

        float coneLimit = coneAngleLimit * Mathf.PI / 180f;
        float lowerTwist = minTwist * Mathf.PI / 180f;
        float upperTwist = maxTwist * Mathf.PI / 180f;

        BallAndSocket constraint = new BallAndSocket(coneLimit, lowerTwist, upperTwist);

        CustomQuaternion constrained = constraint.Constrain(requested);

        CustomQuaternion completedRotation = CustomQuaternion.Multiply(constrained, constrained);

    }
}
