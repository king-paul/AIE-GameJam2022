using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    #region internal classes/structs
    public struct ViewCastInfo
    {
        public bool m_hit;
        public Vector3 m_point;
        public float m_distance;
        public float m_angle;

        public ViewCastInfo(bool hit, Vector3 point, float disance, float angle)
        {
            m_hit = hit;
            m_point = point;
            m_distance = disance;
            m_angle = angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 m_pointA;
        public Vector3 m_pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            m_pointA = pointA;
            m_pointB = pointB;
        }
    }
    #endregion

    #region variables and arrays
    // public variables

    [Tooltip("The Cone Mesh")]
    public MeshFilter viewMeshFilter;

    [Header("Vision Size")]
    [Range(1, 50)]
    public float viewRadius = 2.5f;
    [Range(0,360)]
    public float viewAngle = 60f;

    [Header("Masks")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    
    public List<Transform> visibleTargets = new List<Transform>();

    [Range(0.1f, 1.0f)]
    public float meshResolution = 1f;
    public int edgeResolveIterations = 1;
    public float edgeDistanceThreshold = 1;    

    // private variables
    Mesh viewMesh;
    EnemyController controller;
    #endregion

    #region public functions
    /// <summary>
    /// 
    /// </summary>
    /// <param name="angleInDegrees"></param>
    /// <param name="angleInGlobal"></param>
    /// <returns></returns>
    public Vector3 DirFromAngle(float angleInDegrees, bool angleInGlobal)
    {
        if (!angleInGlobal)
        {
            angleInDegrees += transform.rotation.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    #endregion

    #region start & update functions
    void Start()
    {
        controller = GetComponent<EnemyController>();

        viewMesh = new Mesh();
        viewMesh.name = "ViewMesh";
        viewMeshFilter.mesh = viewMesh;
        viewMesh.RecalculateNormals();

        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    private void Update()
    {
        /*if(PlayerInVision())
        {
            Debug.Log("Enemy can see player");
        }*/
        //FindVisibleTargets();
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }
    #endregion

    #region private functions
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true) {
            yield return new WaitForSeconds(delay);

            //FindVisibleTargets();

            //if(visibleTargets.Count > 0)
                //Debug.Log("Detected Target: " + visibleTargets[0]);

            if(PlayerInVision() && controller.State != EnemyState.Follow)
            {
                //Debug.Log("Enemy can see player");
                controller.State = EnemyState.Follow;
            }
        }
    }

    void FindVisibleTargets() {

        visibleTargets.Clear();

        // get all targets within vision radius
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distanceTorTaget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToTarget, distanceTorTaget, obstacleMask)) {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    /// <summary>
    /// Checks if an object in the player layer is withing the 
    /// </summary>
    /// <returns>true or false based on whether the enemy can see the player</returns>
    bool PlayerInVision()
    {
        Collider[] targetsInViewRadius = 
            Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        if(targetsInViewRadius.Length > 0)
        {
            Transform target = targetsInViewRadius[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distanceTorTaget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distanceTorTaget, obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Draws the vision cone infront of the player
    /// </summary>
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        // create viewpoints based on teh step count
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);

            if(i > 0) {
                // check the distance threshold in cases the ray is cast on 2 objects
                bool edgeThresholdExceeded = 
                    Mathf.Abs(oldViewCast.m_distance - newViewCast.m_distance) > edgeDistanceThreshold;

                if (oldViewCast.m_hit != newViewCast.m_hit || (oldViewCast.m_hit && newViewCast.m_hit && edgeThresholdExceeded)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                    if (edge.m_pointA != Vector3.zero)                    
                        viewPoints.Add(edge.m_pointA);
                    
                    if (edge.m_pointB != Vector3.zero)                    
                        viewPoints.Add(edge.m_pointB);                    
                }
            }

            //ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.m_point);
            oldViewCast = newViewCast;
        }

        // add triangles
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2) * 3]; // number of vertices in 3 tirangles

        // set vertex positions
        vertices[0] = Vector3.zero;

        // and vertices from viewpoint positions
        for(int i=0; i < vertexCount-1; i++)
        {
            vertices[i+1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0; // first triangle
                triangles[i * 3 + 1] = i + 1; // second triangle
                triangles[i * 3 + 2] = i + 2; // third triangle
            }
        }

        // clear the mesh then assigne new vertices and triangles
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        // check if view case hit something
        if(Physics.Raycast(transform.position, direction, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else  { // otherwise draw the maximum distance

            return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
        }
    }

    // detects edges of solid objects usin minimum and maxmimum angles
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.m_angle;
        float maxAngle = maxViewCast.m_angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeThresholdExceeded =
                    Mathf.Abs(minViewCast.m_distance - newViewCast.m_distance) > edgeDistanceThreshold;

            if (newViewCast.m_hit == minViewCast.m_hit && !edgeThresholdExceeded )
            {
                minAngle = angle;
                minPoint = newViewCast.m_point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.m_point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }
    #endregion

    #region gizmos
    void OnDrawGizmos()
    {        
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

        Gizmos.color = Color.green;
        foreach(Transform visibleTarget in visibleTargets)
        {
            Gizmos.DrawLine(transform.position, visibleTarget.position);
        }
    }
    #endregion
}
