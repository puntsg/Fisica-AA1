using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private Fluid fluid;

    private Body body;

    void Start()
    {
        body = GetComponent<Body>();
        if (fluid == null) 
            fluid = FindAnyObjectByType<Fluid>();
        PhysicsManager.instance.Register(body);
    }

    void OnDestroy()
    {
        if (PhysicsManager.instance != null)
            PhysicsManager.instance.Unregister(body);
    }

    void FixedUpdate()
    {
        ApplyFluidForces();
    }

    void ApplyFluidForces()
    {
        float submergedVolume = CalculateSubmergedVolume();
        if (submergedVolume <= 0f)
            return;

        //Principio de Arquimedes : Empuje = densidad_fluido * volumen_sumergido * gravedad
        Vector3 buoyancy = new Vector3(0f, fluid.density * submergedVolume * PhysicsManager.instance.gravity, 0f);
        //Fuerza de arrastre : Fuerza = -coeficiente * volumen_sumergido * velocidad
        Vector3 drag = -fluid.dragCoefficient * submergedVolume * body.targetvelocity;

        body.targetnetForce += buoyancy + drag;
    }

    float CalculateSubmergedVolume()
    {
        float surfaceHeight = fluid.GetSurfaceHeight(body.targetposition);
        float objectBottomY = body.targetposition.y - 0.5f * body.size.y;
        float objectTopY    = body.targetposition.y + 0.5f * body.size.y;

        if (objectBottomY >= surfaceHeight) 
            return 0f;
        if (objectTopY   <= surfaceHeight) 
            return body.size.x * body.size.y * body.size.z;

        float submergedHeight = surfaceHeight - objectBottomY;
        return body.size.x * submergedHeight * body.size.z;
    }
}
