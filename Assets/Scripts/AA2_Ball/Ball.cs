using UnityEngine;

public class Ball : MonoBehaviour
{
    public Dragger dragger;
    Body body;

    void Start()
    {
        body = GetComponent<Body>();
        PhysicsManager.instance.Register(body);
        dragger.onFinishDrag.AddListener((Vector2 v) => { Launch(v); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            body.ResetBody();
            AA2Params.instance.ResetParams();
            GeneralParams.instance.ResetParams();
        }
        if (Input.GetKeyDown(KeyCode.P))
            body.targetvelocity = Vector3.zero;
    }

    void Launch(Vector2 dragValue)
    {
        Vector3 right = Camera.main.transform.right;
        Vector3 forward = Camera.main.transform.forward;
        right.y = 0;
        forward.y = 0;
        right.Normalize();
        forward.Normalize();
        Vector3 direction = right * dragValue.x + forward * dragValue.y;
        //Segunda ley de Newton : Fuerza = masa * aceleracion --> aceleracion = Fuerza / masa 
        body.targetvelocity = direction * AA2Params.instance.initialForce / body.mass;
    }
}
