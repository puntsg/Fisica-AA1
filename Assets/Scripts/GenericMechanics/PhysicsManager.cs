using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager instance;

    public float gravity = 9.81f;
    public Vector3 gravityDirection = Vector3.down;
    public float stepTime = 0.01f;

    private List<Body> bodies = new List<Body>();

    public Vector3 GravityVector => gravity * gravityDirection.normalized;

    private void Awake() { instance = this; }

    public void Register(Body body) { bodies.Add(body); }
    public void Unregister(Body body) { bodies.Remove(body); }

    void FixedUpdate()
    {
        foreach (Body body in bodies)
        {
            //Peso (Segunda ley de Newton) : Fuerza = masa * gravedad
            body.targetnetForce += GravityVector * body.mass;
            body.UpdateBody();
        }
    }
}
