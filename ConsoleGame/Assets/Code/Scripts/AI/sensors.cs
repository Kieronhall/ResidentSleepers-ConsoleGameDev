using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensors : MonoBehaviour
{
    public LayerMask hitMask;
    public LayerMask obstacleMask; 
    public enum Type
    {
        Line,
        RayBundle,
        SphereCast,
        BoxCast,
        BoxCast_2
    }
    public Type sensorType = Type.Line;
    public float raycastLength = 1.0f;
    public float sensorHeight = 0.0f;

    [Header("BoxExtent Settings")]
    public Vector2 boxExtents = new Vector2(1.0f, 1.0f);

    [Header("Sphere Settings")]
    public float sphereRadius = 1.0f;

    [Header("RayBundle Settings")]
    [Range(1, 100)]
    public int rayCount = 5;
    [Range(0, 360)]
    public int searchArc = 90;

    Transform cachedTransform;

    void Start()
    {
        cachedTransform = GetComponent<Transform>();
    }

    void Update()
    {
        Scan();
        if (Hit == true)
        {
            raycastLength = 10;
            searchArc = 180;
            rayCount = 45;
        }
        else
        {
            raycastLength = 5;
            searchArc = 90;
            rayCount = 45;
        }
    }

    public bool Hit { get; private set; }
    public RaycastHit info = new RaycastHit();
    public RaycastHit[] hits;

    public bool Scan()
    {
        Hit = false;
        Vector3 dir = cachedTransform.forward;
        Vector3 sensorPosition = cachedTransform.position + Vector3.up * sensorHeight;
        bool playerHitDetected = false;
        RaycastHit closestPlayerHit = new RaycastHit();
        float closestPlayerDistance = float.MaxValue;
        RaycastHit tempHitInfo;

        switch (sensorType)
        {
            case Type.Line:
                if (Physics.Linecast(sensorPosition, sensorPosition + dir * raycastLength, out info, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.RayBundle:
                float startSweep = -searchArc * 0.5f;
                float sweepGap = searchArc / (float)rayCount;
                for (int i = 0; i <= rayCount; i++)
                {
                    dir = (Quaternion.Euler(0, startSweep + i * sweepGap, 0) * cachedTransform.forward).normalized;
                    Vector3 targetPosition = sensorPosition + dir * raycastLength;

                    if (Physics.Raycast(sensorPosition, dir, out tempHitInfo, raycastLength, hitMask))
                    {
                        float distanceToPlayer = tempHitInfo.distance;
                        if (distanceToPlayer < closestPlayerDistance)
                        {
                            // checking for obstacle between sensor and player
                            bool obstacleBetween = Physics.Raycast(sensorPosition, dir, distanceToPlayer, obstacleMask);
                            if (!obstacleBetween)
                            {
                                closestPlayerHit = tempHitInfo;
                                closestPlayerDistance = distanceToPlayer;
                                playerHitDetected = true;
                            }
                        }
                    }
                }
                if (playerHitDetected)
                {
                    Hit = true;
                    info = closestPlayerHit;
                }
                break;
            case Type.SphereCast:
                if (Physics.SphereCast(new Ray(sensorPosition, dir), sphereRadius, out info, raycastLength, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.BoxCast:
                if (Physics.CheckBox(sensorPosition, new Vector3(boxExtents.x, boxExtents.y, raycastLength) / 2.0f, this.transform.rotation, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
            case Type.BoxCast_2:
                if (Physics.CheckBox(new Vector3(0,0,raycastLength), new Vector3(boxExtents.x, boxExtents.y, 1) / 2.0f, this.transform.rotation, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (cachedTransform == null)
        {
            cachedTransform = GetComponent<Transform>();
        }

        // applying sensor height
        Vector3 sensorPosition = cachedTransform.position + Vector3.up * sensorHeight;

        Scan();
        if (Hit) Gizmos.color = Color.red;
        Gizmos.matrix *= Matrix4x4.TRS(sensorPosition, transform.rotation, Vector3.one);

        float length = raycastLength;

        switch (sensorType)
        {
            case Type.Line:
                if (Hit) length = Vector3.Distance(sensorPosition, info.point);
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(Vector3.forward * length, new Vector3(0.02f, 0.02f, 0.02f));
                break;
            case Type.RayBundle:
                if (Hit)
                    length = Vector3.Distance(sensorPosition, info.point);
                float startSweep = -searchArc * 0.5f;
                float finishSweep = searchArc * 0.5f;
                float sweepGap = searchArc / rayCount;
                for (int i = 0; i < rayCount + 1; i++)
                {
                    Vector3 dir = (Quaternion.Euler(0, startSweep + i * sweepGap, 0) * Vector3.forward).normalized;
                    Gizmos.DrawLine(Vector3.zero, dir * length);
                }
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.02f, 0.02f, 0.02f));
                Gizmos.color = Color.green;
                if (Hit)
                {
                    Gizmos.matrix = Matrix4x4.identity;
                    Gizmos.DrawWireCube(info.point, new Vector3(0.02f, 0.02f, 0.02f));
                }
                break;
            case Type.SphereCast:
                Gizmos.DrawWireSphere(Vector3.zero, sphereRadius);
                if (Hit)
                {
                    Vector3 ballCenter = info.point + info.normal * sphereRadius;
                    length = Vector3.Distance(sensorPosition, ballCenter);
                }
                float halfExtents = sphereRadius;
                Gizmos.DrawLine(Vector3.up * halfExtents, Vector3.up * halfExtents + Vector3.forward * length);
                Gizmos.DrawLine(-Vector3.up * halfExtents, -Vector3.up * halfExtents + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.right * halfExtents, Vector3.right * halfExtents + Vector3.forward * length);
                Gizmos.DrawLine(-Vector3.right * halfExtents, -Vector3.right * halfExtents + Vector3.forward * length);
                Gizmos.DrawWireSphere(Vector3.zero + Vector3.forward * length, sphereRadius);
                break;
            case Type.BoxCast:
                Vector3 boxHalfExtents = new Vector3(boxExtents.x, boxExtents.y, raycastLength) / 2.0f;
                Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2f);
                break;
            case Type.BoxCast_2:
                Vector3 boxHalfExtents2 = new Vector3(boxExtents.x, boxExtents.y, 1) / 2.0f;
                Gizmos.DrawWireCube(new Vector3(0, 0, raycastLength), boxHalfExtents2 * 2f);
                break;            
        }
    }
}
