using UnityEngine;

public class BallAndSocket 
{
    private float coneAngle;
    private float minTwist;
    private float maxTwist;

    public BallAndSocket(float coneAngle, float minTwist, float maxTwist    )
    {
        this.coneAngle = coneAngle;
        this.minTwist = minTwist;
        this.maxTwist = maxTwist;
    }
    public CustomQuaternion Constrain(CustomQuaternion requested)
    {
        CustomQuaternion q_twist, q_swing;
        Spring.Decompose(requested, out q_swing, out q_twist);
        Vector2 swing = Spring.GetAxisSwing(q_swing);
        float swingAngle = (float)Mathf.Sqrt(swing.x * swing.x + swing.y * swing.y);
        if(swingAngle > coneAngle && swingAngle > 0)
        {
            float scale = coneAngle / swingAngle;
            swing.x = scale;
            swing.y = scale;
        }
        float twistAngle = Spring.GetTwistedAngle(q_twist);
        twistAngle = Spring.Clamp(twistAngle, minTwist,maxTwist);

        q_swing = Spring.FromSwingCoordinates(swing);
        q_twist = new CustomQuaternion(twistAngle,0,0,1);

        CustomQuaternion q = CustomQuaternion.Multiply(q_twist, q_swing);
        q.Normalize();

        return q;
    }
}
