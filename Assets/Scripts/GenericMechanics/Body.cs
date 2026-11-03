using UnityEngine;

public class Body : MonoBehaviour
{
    public float mass = 1f;
    public Vector3 size = Vector3.one;
    public Vector3 initialPosition = Vector3.zero;
    public Vector3 initialVelocity = Vector3.zero;
    public Vector3 initialRotation = Vector3.zero;

    public Vector3 targetposition;
    public Vector3 targetvelocity;
    public Vector3 targetnetForce;
    public Vector3 acceleration;

    public enum EIntegrationMode { Euler, Verlet }
    public EIntegrationMode integrationMode = EIntegrationMode.Euler;

    protected virtual void Start()
    {
        targetposition = initialPosition;
        targetvelocity = initialVelocity;
        transform.position = targetposition;
    }

    public void UpdateBody()
    {
        //https://upcommons.upc.edu/entities/publication/d35eb57c-cad3-4eed-8e09-e9c68be85745
        //https://upcommons.upc.edu/server/api/core/bitstreams/10ecac9f-1a2e-455a-a4a1-1cd1159840ce/content
        //Segunda ley de Newton : Fuerza = masa * aceleracion  ->  aceleracion = Fuerza / masa
        Vector3 acc = targetnetForce / mass;
        float dt = Time.deltaTime * GeneralParams.instance.timeMultiplier;
        if (integrationMode == EIntegrationMode.Verlet)
        {
            //Integracion de Verlet : posicion = posicion + velocidad*dt + 0.5 * aceleracion * dt^2
            targetposition += targetvelocity * dt + 0.5f * acc * dt * dt;
            targetvelocity += acc * dt;
        }
        else
        {
            //Euler: velocidad = velocidad + aceleracion*dt ; posicion = posicion + velocidad*dt
            targetvelocity += acc * dt;
            targetposition += targetvelocity * dt;
        }
        transform.position = targetposition;
        targetnetForce = Vector3.zero;
    }

    public void UpdateVelocity(float dt)
    {
        //Segunda ley de Newton : aceleracion = Fuerza / masa
        Vector3 newAcceleration = targetnetForce / mass;
        //Velocity Verlet (velocidad) : velocidad = velocidad + 0.5 * (aceleracion_vieja + aceleracion_nueva) * dt
        targetvelocity += 0.5f * (acceleration + newAcceleration) * dt;
        acceleration = newAcceleration;
        targetnetForce = Vector3.zero;
    }
    //Aqui el codigo esta duplicado porque acabe mezclando un timeStep propio y el nativo de unity(Time.deltTime) y petaba un poco al modificar la escala de tiempo
    public void UpdatePosition(float dt)
    {
        //Velocity Verlet (posicion) : posicion = posicion + velocidad*dt + 0.5 * aceleracion * dt^2
        targetposition += targetvelocity * dt + 0.5f * acceleration * dt * dt;
        transform.position = targetposition;
    }

    public virtual void ResetBody()
    {
        targetposition = initialPosition;
        targetvelocity = initialVelocity;
        acceleration = Vector3.zero;
        targetnetForce = Vector3.zero;
        transform.position = initialPosition;
    }
}
