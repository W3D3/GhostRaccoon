using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    /**
     * Defines mesh resolution
     */
    public float resolution;

    public MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    public int edgeDetectionAccuracy = 6;
    public float edgeDistanceThreshold =  .5f;
    
    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        
        StartCoroutine("FindTargetsWithDelay", .1f);
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    IEnumerator FindTargetsWithDelay(float delaySec)
    {
        while (true)
        {
            yield return new WaitForSeconds(delaySec);
            FindVisibleTargets();
        }
    }
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var targetCollider in targetsInViewRadius)
        {
            var target = targetCollider.transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dirToTarget, distance, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * resolution);
        float stepAngleSize = viewAngle / stepCount;
        ViewCastInfo oldViewCast = new ViewCastInfo();

        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            // Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.magenta);
            ViewCastInfo viewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistanceThresholdExceeded =
                    Mathf.Abs(oldViewCast.distance - viewCast.distance) > edgeDistanceThreshold;
                if (oldViewCast.hit != viewCast.hit || (oldViewCast.hit && viewCast.hit && edgeDistanceThresholdExceeded))
                {
                    var edge = FindEdgeBetween(oldViewCast, viewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                        // Debug.DrawLine(transform.position, edge.pointA, Color.green);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                        // Debug.DrawLine(transform.position, edge.pointB, Color.green);
                    }
                }
            }
            viewPoints.Add(viewCast.point);
            oldViewCast = viewCast;
        }

        int vertexCount = viewPoints.Count + 1; // +1 for origin
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero; // local origin
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    EdgeInfo FindEdgeBetween(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeDetectionAccuracy; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo midViewCast = ViewCast(angle);
            bool edgeDistanceThresholdExceeded =
                Mathf.Abs(minViewCast.distance - midViewCast.distance) > edgeDistanceThreshold;

            if (midViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                // move the min to newly found mid
                minAngle = angle;
                minPoint = midViewCast.point;
            }
            else
            {
                // move the max to newly found mid
                maxAngle = angle;
                maxPoint = midViewCast.point;
            }
        }
        
        return new EdgeInfo(minPoint, maxPoint);
        
    }
    
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        // We swap Cos and Sin because in Unity the unit circle starts on top and goes clockwise
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    public struct ViewCastInfo {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }
    
    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}
