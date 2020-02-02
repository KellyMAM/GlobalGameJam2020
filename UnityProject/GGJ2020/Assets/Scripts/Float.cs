using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshFilter))]
public class Float : MonoBehaviour
{
    [SerializeField]
    private bool calculateDensity = false;

    [SerializeField]
    private float density = 0.75f;
    [SerializeField]
    private float viscosity = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float normalizedVoxelSize = 0.5f;

    [SerializeField]
    private float dragInWater = 1f;

    [SerializeField]
    private float angularDragInWater = 1f;

    [SerializeField]
    private Transform water;
    private Collider collider;
    private Rigidbody rigidbody;
    private float initialDrag;
    private float initialAngularDrag;
    private Vector3 voxelSize;
    private Vector3[] voxels;

    private string waterVolumetag = "water";
    protected virtual void Awake()
    {
        this.collider = this.GetComponent<Collider>();
        this.rigidbody = this.GetComponent<Rigidbody>();

        this.initialDrag = this.rigidbody.drag;
        this.initialAngularDrag = this.rigidbody.angularDrag;
        this.voxels = new Vector3[0];
        if(water == null) {
            water = GameObject.FindGameObjectsWithTag(waterVolumetag)[0].transform;
        }

        voxels = this.CutIntoVoxels();

        if (this.calculateDensity)
        {
            float objectVolume = CalculateVolume_Mesh(this.GetComponent<MeshFilter>().mesh, this.transform);
            this.density = this.rigidbody.mass / objectVolume;
        }
    }
    public bool IsPointInsideCollider(Vector3 point, Collider collider, ref Bounds colliderBounds)
    {
        float rayLength = colliderBounds.size.magnitude;
        Ray ray = new Ray(point, collider.transform.position - point);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider == collider)
            {
                return false;
            }
        }

        return true;
    }

    public float CalculateVolume_Mesh(Mesh mesh, Transform trans)
    {
        float volume = 0f;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];

            volume += CalculateVolume_Tetrahedron(p1, p2, p3, Vector3.zero);
        }

        return Mathf.Abs(volume) * trans.localScale.x * trans.localScale.y * trans.localScale.z;
    }

    public float CalculateVolume_Tetrahedron(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        Vector3 a = p1 - p2;
        Vector3 b = p1 - p3;
        Vector3 c = p1 - p4;

        return (Vector3.Dot(a, Vector3.Cross(b, c))) / 6f;
    }

    protected virtual void FixedUpdate()
    {
        Debug.Log(water);
        Debug.Log(voxels);
        if (this.water != null && this.voxels.Length > 0)
        {
            Debug.Log(this.voxels);
            Vector3 forceAtSingleVoxel = this.CalculateMaxBuoyancyForce() / this.voxels.Length;
            Bounds bounds = this.collider.bounds;
            float voxelHeight = bounds.size.y * this.normalizedVoxelSize;

            float submergedVolume = 0f;
            for (int i = 0; i < this.voxels.Length; i++)
            {
                Vector3 worldPoint = this.transform.TransformPoint(this.voxels[i]);

                float waterLevel = this.water.position.y;
                float deepLevel = waterLevel - worldPoint.y + (voxelHeight / 2f); // How deep is the voxel
                float submergedFactor = Mathf.Clamp(deepLevel / voxelHeight, 0f, 1f); // 0 - voxel is fully out of the water, 1 - voxel is fully submerged
                submergedVolume += submergedFactor;

                Vector3 surfaceNormal = Vector3.up;
                Quaternion surfaceRotation = Quaternion.FromToRotation(this.water.up, surfaceNormal);
                surfaceRotation = Quaternion.Slerp(surfaceRotation, Quaternion.identity, submergedFactor);

                Vector3 finalVoxelForce = surfaceRotation * (forceAtSingleVoxel * submergedFactor);
                this.rigidbody.AddForceAtPosition(finalVoxelForce, worldPoint);

                Debug.DrawLine(worldPoint, worldPoint + finalVoxelForce.normalized, Color.blue);
            }

            submergedVolume /= this.voxels.Length; // 0 - object is fully out of the water, 1 - object is fully submerged

            this.rigidbody.drag = Mathf.Lerp(this.initialDrag, this.dragInWater, submergedVolume);
            this.rigidbody.angularDrag = Mathf.Lerp(this.initialAngularDrag, this.angularDragInWater, submergedVolume);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(waterVolumetag))
        {
            // this.water = other.GetComponent<WaterVolume>();
            if (this.voxels == null)
            {
                this.voxels = this.CutIntoVoxels();
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(waterVolumetag))
        {
            this.water = null;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (this.voxels != null)
        {
            for (int i = 0; i < this.voxels.Length; i++)
            {
                Gizmos.color = Color.magenta - new Color(0f, 0f, 0f, 0.75f);
                Gizmos.DrawCube(this.transform.TransformPoint(this.voxels[i]), this.voxelSize * 0.8f);
            }
        }
    }

    private Vector3 CalculateMaxBuoyancyForce()
    {
        float objectVolume = this.rigidbody.mass  / this.density;
        Vector3 maxBuoyancyForce = viscosity * objectVolume * -Physics.gravity;

        return maxBuoyancyForce;
    }

    private Vector3[] CutIntoVoxels()
    {
        Quaternion initialRotation = this.transform.rotation;
        this.transform.rotation = Quaternion.identity;

        Bounds bounds = this.collider.bounds;
        this.voxelSize.x = bounds.size.x * this.normalizedVoxelSize;
        this.voxelSize.y = bounds.size.y * this.normalizedVoxelSize;
        this.voxelSize.z = bounds.size.z * this.normalizedVoxelSize;
        int voxelsCountForEachAxis = Mathf.RoundToInt(1f / this.normalizedVoxelSize);
        List<Vector3> voxels = new List<Vector3>(voxelsCountForEachAxis * voxelsCountForEachAxis * voxelsCountForEachAxis);
        for (int i = 0; i < voxelsCountForEachAxis; i++)
        {
            for (int j = 0; j < voxelsCountForEachAxis; j++)
            {
                for (int k = 0; k < voxelsCountForEachAxis; k++)
                {
                    float pX = bounds.min.x + this.voxelSize.x * (0.5f + i);
                    float pY = bounds.min.y + this.voxelSize.y * (0.5f + j);
                    float pZ = bounds.min.z + this.voxelSize.z * (0.5f + k);

                    Vector3 point = new Vector3(pX, pY, pZ);
                    if (IsPointInsideCollider(point, this.collider, ref bounds))
                    {
                        voxels.Add(this.transform.InverseTransformPoint(point));
                    }
                }
            }
        }

        this.transform.rotation = initialRotation;

        return voxels.ToArray();
    }
}