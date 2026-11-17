using System.Collections.Generic;
using UnityEngine;

public class AstralBodyManager : MonoBehaviour
{
    public static AstralBodyManager instance;

    public float fixedStep = 0.01f;
    public int maxStepsPerFrame = 200;

    private List<AstralBody> astralBodies = new List<AstralBody>();
    private bool initialized = false;
    private float accumulator;

    private void Awake() { instance = this; }

    public void AddBody(AstralBody astralBody)
    {
        if (!astralBodies.Contains(astralBody))
            astralBodies.Add(astralBody);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (AstralBody a in astralBodies)
                a.ResetBody();
            AA1Params.instance.ResetParams();
            GeneralParams.instance.ResetParams();
            initialized = false;
            accumulator = 0f;
        }

        if (!initialized)
        {
            ComputeGravity();
            foreach (AstralBody a in astralBodies)
            {
                a.acceleration = a.targetnetForce / a.mass;
                a.targetnetForce = Vector3.zero;
            }
            initialized = true;
        }

        accumulator += Time.deltaTime * GeneralParams.instance.timeMultiplier;

        int steps = 0;
        while (accumulator >= fixedStep && steps < maxStepsPerFrame)
        {
            Step(fixedStep);
            accumulator -= fixedStep;
            steps++;
        }
    }

    void Step(float dt)
    {
        foreach (AstralBody a in astralBodies)
            a.UpdatePosition(dt);

        ComputeGravity();

        foreach (AstralBody a in astralBodies)
            a.UpdateVelocity(dt);
    }

    private void ComputeGravity()
    {
        for (int i = 0; i < astralBodies.Count; i++)
        {
            for (int j = i + 1; j < astralBodies.Count; j++)
            {
                Vector3 direction = astralBodies[j].targetposition - astralBodies[i].targetposition;
                float distance = direction.magnitude;
                if (distance < 0.001f) continue;

                //Ley de Gravitacion Universal de Newton : Fuerza = G * masa1 * masa2 / distancia^2
                float forceMagnitude = AA1Params.instance.G * astralBodies[i].mass * astralBodies[j].mass / (distance * distance);
                Vector3 force = direction.normalized * forceMagnitude;

                astralBodies[i].targetnetForce += force;
                astralBodies[j].targetnetForce -= force;
            }
        }
    }
}
