using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager instance;
    public List<AABB> colliders = new List<AABB>();

    private void Awake(){instance = this;}

    void FixedUpdate()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].collidedObjects.Clear();
            colliders[i].triggered = false;
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            for (int j = i + 1; j < colliders.Count; j++)
            {
                AABB a = colliders[i];
                AABB b = colliders[j];
                if (a.CollidesWith(b))
                {
                    a.collidedObjects.Add(b.gameObject);
                    b.collidedObjects.Add(a.gameObject);
                    a.triggered = true;
                    b.triggered = true;
                    ResolveCollision(a, b);
                }
            }
        }
    }

    void ResolveCollision(AABB a, AABB b)
    {
        AABB sphere = null;
        AABB box = null;

        if (a.collisionShape == AABB.ECollisionShape.SPHERE && b.collisionShape == AABB.ECollisionShape.CUBIC)
        {
            sphere = a;
            box = b;
        }
        if (a.collisionShape == AABB.ECollisionShape.CUBIC && b.collisionShape == AABB.ECollisionShape.SPHERE)
        {
            sphere = b;
            box = a;
        }
        if (sphere == null) return;

        if (sphere.TryGetComponent(out Body body))
        {
            Vector3 normal;
            float penetration;
            if (sphere.SphereBoxCollision(box, out normal, out penetration))
            {
                body.targetposition += normal * penetration;
                body.transform.position = body.targetposition;

                float vn = Vector3.Dot(body.targetvelocity, normal);
                if (vn < 0f)
                {
                    Vector3 vNormal = vn * normal;
                    Vector3 vTangent = body.targetvelocity - vNormal;

                    float restitution = 0f;
                    float friction = 0f;
                    if (box.TryGetComponent(out Surface surface))
                    {
                        friction = surface.friction;
                        restitution = surface.restitution;
                    }
                    friction = Mathf.Clamp01(friction);

                    //Rozamiento viscoso (decaimiento exponencial) + coef. de restitucion : v = v_tangente * (1 - friccion)^dt  -  v_normal * restitucion
                    body.targetvelocity = vTangent * Mathf.Pow(1f - friction, Time.fixedDeltaTime) - vNormal * restitution;
                }
            }
        }
    }
}
