using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AABB : MonoBehaviour
{
    [Serializable] public enum ECollisionShape { CUBIC, SPHERE }

    public ECollisionShape collisionShape;

    public bool debugAABB;
    public float sphereRadius;
    public Vector3 boxSize;

    public bool collisionEnabled, triggered;

    public List<GameObject> collidedObjects;
    public UnityEvent<GameObject> oncollisionEntrer;
    public UnityEvent<GameObject> oncollisionExit;

    private void Start(){CollisionManager.instance.colliders.Add(this); }
    private void OnDestroy(){ 
        if (CollisionManager.instance != null) 
            CollisionManager.instance.colliders.Remove(this); 
    }

    public bool CollidesWith(AABB other)
    {
        if (collisionShape == ECollisionShape.SPHERE && other.collisionShape == ECollisionShape.SPHERE)
            return SphereVsSphere(other);
        if (collisionShape == ECollisionShape.SPHERE && other.collisionShape == ECollisionShape.CUBIC)
            return SphereVsBox(other);
        if (collisionShape == ECollisionShape.CUBIC && other.collisionShape == ECollisionShape.SPHERE)
            return other.SphereVsBox(this);
        return BoxVsBox(other);
    }

    bool SphereVsSphere(AABB other)
    {
        //Colision esfera-esfera : Choque = distancia entre centros < radio1 + radio2
        float dx = other.transform.position.x - transform.position.x;
        float dy = other.transform.position.y - transform.position.y;
        float dz = other.transform.position.z - transform.position.z;
        float distance = Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
        float radius = sphereRadius + other.sphereRadius;
        return (distance < radius);
    }

    bool BoxVsBox(AABB other)
    {
        //Colision caja-caja (AABB) : solapamiento min y max en los 3 ejes
        Vector3 aMin = transform.position - boxSize / 2.0f;
        Vector3 aMax = transform.position + boxSize / 2.0f;
        Vector3 bMin = other.transform.position - other.boxSize / 2.0f;
        Vector3 bMax = other.transform.position + other.boxSize / 2.0f;

        if (aMin.x > bMax.x)
            return false;
        if (aMax.x < bMin.x)
            return false;
        if (aMin.y > bMax.y) 
            return false;
        if (aMax.y < bMin.y)
            return false;
        if (aMin.z > bMax.z) 
            return false;
        if (aMax.z < bMin.z) 
            return false;
        return true;
    }

    bool SphereVsBox(AABB box)
    {
        Vector3 normal;
        float penetration;
        return SphereBoxCollision(box, out normal, out penetration);
    }

    public bool SphereBoxCollision(AABB box, out Vector3 normal, out float penetration)
    {
        Matrix4x4 m = Matrix4x4.TRS(box.transform.position, box.transform.rotation, Vector3.one);
        Vector3 local = m.inverse.MultiplyPoint3x4(transform.position);

        Vector3 half = box.boxSize / 2.0f;
        Vector3 clamped = local;
        clamped.x = Mathf.Clamp(local.x, -half.x, half.x);
        clamped.y = Mathf.Clamp(local.y, -half.y, half.y);
        clamped.z = Mathf.Clamp(local.z, -half.z, half.z);

        //Colision esfera-caja (OBB) :  distancia < radio = colision
        Vector3 closest = m.MultiplyPoint3x4(clamped);
        Vector3 diff = transform.position - closest;
        float distance = diff.magnitude;

        normal = box.transform.up;
        penetration = 0f;
        if (distance >= sphereRadius) 
            return false;
        if (distance > 0.00001f) 
            normal = diff / distance;
        penetration = sphereRadius - distance;
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        if (collisionShape == ECollisionShape.SPHERE)
            Gizmos.DrawWireSphere(Vector3.zero, sphereRadius);
        else
            DrawCube(Vector3.zero, boxSize);
        Gizmos.matrix = Matrix4x4.identity;
    }

    void DrawCube(Vector3 center, Vector3 dimensions)
    {
        Vector3[] corners = new Vector3[8]
        {
            center + new Vector3(-0.5f * dimensions.x, -0.5f * dimensions.y, -0.5f * dimensions.z),
            center + new Vector3( 0.5f * dimensions.x, -0.5f * dimensions.y, -0.5f * dimensions.z),
            center + new Vector3( 0.5f * dimensions.x,  0.5f * dimensions.y, -0.5f * dimensions.z),
            center + new Vector3(-0.5f * dimensions.x,  0.5f * dimensions.y, -0.5f * dimensions.z),
            center + new Vector3(-0.5f * dimensions.x, -0.5f * dimensions.y,  0.5f * dimensions.z),
            center + new Vector3( 0.5f * dimensions.x, -0.5f * dimensions.y,  0.5f * dimensions.z),
            center + new Vector3( 0.5f * dimensions.x,  0.5f * dimensions.y,  0.5f * dimensions.z),
            center + new Vector3(-0.5f * dimensions.x,  0.5f * dimensions.y,  0.5f * dimensions.z),
        };

        // back face
        Gizmos.DrawLine(corners[0], corners[1]);
        Gizmos.DrawLine(corners[1], corners[2]);
        Gizmos.DrawLine(corners[2], corners[3]);
        Gizmos.DrawLine(corners[3], corners[0]);
        // front face
        Gizmos.DrawLine(corners[4], corners[5]);
        Gizmos.DrawLine(corners[5], corners[6]);
        Gizmos.DrawLine(corners[6], corners[7]);
        Gizmos.DrawLine(corners[7], corners[4]);
        // connecting edges
        Gizmos.DrawLine(corners[0], corners[4]);
        Gizmos.DrawLine(corners[1], corners[5]);
        Gizmos.DrawLine(corners[2], corners[6]);
        Gizmos.DrawLine(corners[3], corners[7]);
    }
}
