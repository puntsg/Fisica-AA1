using UnityEngine;

public class AstralBody : Body
{
    public Color trailColor = Color.white;
    public float trailTime = 5f;
    public float trailWidth = 0.1f;

    private TrailRenderer trail;

    protected override void Start()
    {
        base.Start();
        AstralBodyManager.instance.AddBody(this);

        trail = gameObject.AddComponent<TrailRenderer>();
        trail.time = trailTime;
        trail.startWidth = trailWidth;
        trail.endWidth = 0f;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.startColor = trailColor;
        trail.endColor = new Color(trailColor.r, trailColor.g, trailColor.b, 0f);
    }

    public override void ResetBody()
    {
        base.ResetBody();
        if (trail != null) 
            trail.Clear();
    }
}
