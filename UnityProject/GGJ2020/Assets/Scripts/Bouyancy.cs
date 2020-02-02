using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Internal;
using UnityEngine.Assertions.Comparers;

using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Collider))]
// [RequireComponent(typeof(Rigidbody))]
// [RequireComponent(typeof(MeshFilter))]
// public class Bouyancy : MonoBehaviour
// {
//     [SerializeField]
//     private bool calculateDensity = false;

//     [SerializeField]
//     private float density = 0.75f;
//     [SerializeField]
//     private float viscosity = 20f;

//     [SerializeField]
//     [Range(0f, 1f)]
//     private float normalizedVoxelSize = 0.5f;

//     [SerializeField]
//     private float dragInWater = 1f;

//     [SerializeField]
//     private float angularDragInWater = 1f;

//     [SerializeField]
//     private Transform water;
//     private Collider collider;
//     private Rigidbody rigidbody;
//     private float initialDrag;
//     private float initialAngularDrag;
//     private Vector3 voxelSize;
//     private Vector3[] voxels;

//     private string waterVolumetag = "water";
//     protected virtual void Awake()
//     {
//         this.collider = this.GetComponent<Collider>();
//         this.rigidbody = this.GetComponent<Rigidbody>();

//         this.initialDrag = this.rigidbody.drag;
//         this.initialAngularDrag = this.rigidbody.angularDrag;
//         this.voxels = new Vector3[0];

//         this.CutIntoVoxels()

//         if (this.calculateDensity)
//         {
//             float objectVolume = CalculateVolume_Mesh(this.GetComponent<MeshFilter>().mesh, this.transform);
//             this.density = this.rigidbody.mass / objectVolume;
//         }
//     }
//     public bool IsPointInsideCollider(Vector3 point, Collider collider, ref Bounds colliderBounds)
//     {
//         float rayLength = colliderBounds.size.magnitude;
//         Ray ray = new Ray(point, collider.transform.position - point);
//         RaycastHit hit;

//         if (Physics.Raycast(ray, out hit, rayLength))
//         {
//             if (hit.collider == collider)
//             {
//                 return false;
//             }
//         }

//         return true;
//     }

//     public float CalculateVolume_Mesh(Mesh mesh, Transform trans)
//     {
//         float volume = 0f;
//         Vector3[] vertices = mesh.vertices;
//         int[] triangles = mesh.triangles;
//         for (int i = 0; i < mesh.triangles.Length; i += 3)
//         {
//             Vector3 p1 = vertices[triangles[i + 0]];
//             Vector3 p2 = vertices[triangles[i + 1]];
//             Vector3 p3 = vertices[triangles[i + 2]];

//             volume += CalculateVolume_Tetrahedron(p1, p2, p3, Vector3.zero);
//         }

//         return Mathf.Abs(volume) * trans.localScale.x * trans.localScale.y * trans.localScale.z;
//     }

//     public float CalculateVolume_Tetrahedron(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
//     {
//         Vector3 a = p1 - p2;
//         Vector3 b = p1 - p3;
//         Vector3 c = p1 - p4;

//         return (Vector3.Dot(a, Vector3.Cross(b, c))) / 6f;
//     }

//     protected virtual void FixedUpdate()
//     {
//         if (this.water != null && this.voxels.Length > 0)
//         {
//             Debug.Log(this.voxels);
//             Vector3 forceAtSingleVoxel = this.CalculateMaxBuoyancyForce() / this.voxels.Length;
//             Bounds bounds = this.collider.bounds;
//             float voxelHeight = bounds.size.y * this.normalizedVoxelSize;

//             float submergedVolume = 0f;
//             for (int i = 0; i < this.voxels.Length; i++)
//             {
//                 Vector3 worldPoint = this.transform.TransformPoint(this.voxels[i]);

//                 float waterLevel = this.water.position.y;
//                 float deepLevel = waterLevel - worldPoint.y + (voxelHeight / 2f); // How deep is the voxel
//                 float submergedFactor = Mathf.Clamp(deepLevel / voxelHeight, 0f, 1f); // 0 - voxel is fully out of the water, 1 - voxel is fully submerged
//                 submergedVolume += submergedFactor;

//                 Vector3 surfaceNormal = Vector3.up;
//                 Quaternion surfaceRotation = Quaternion.FromToRotation(this.water.up, surfaceNormal);
//                 surfaceRotation = Quaternion.Slerp(surfaceRotation, Quaternion.identity, submergedFactor);

//                 Vector3 finalVoxelForce = surfaceRotation * (forceAtSingleVoxel * submergedFactor);
//                 this.rigidbody.AddForceAtPosition(finalVoxelForce, worldPoint);

//                 Debug.DrawLine(worldPoint, worldPoint + finalVoxelForce.normalized, Color.blue);
//             }

//             submergedVolume /= this.voxels.Length; // 0 - object is fully out of the water, 1 - object is fully submerged

//             this.rigidbody.drag = Mathf.Lerp(this.initialDrag, this.dragInWater, submergedVolume);
//             this.rigidbody.angularDrag = Mathf.Lerp(this.initialAngularDrag, this.angularDragInWater, submergedVolume);
//         }
//     }

//     protected virtual void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag(waterVolumetag))
//         {
//             // this.water = other.GetComponent<WaterVolume>();
//             if (this.voxels == null)
//             {
//                 this.voxels = this.CutIntoVoxels();
//             }
//         }
//     }

//     protected virtual void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag(waterVolumetag))
//         {
//             this.water = null;
//         }
//     }

//     protected virtual void OnDrawGizmos()
//     {
//         if (this.voxels != null)
//         {
//             for (int i = 0; i < this.voxels.Length; i++)
//             {
//                 Gizmos.color = Color.magenta - new Color(0f, 0f, 0f, 0.75f);
//                 Gizmos.DrawCube(this.transform.TransformPoint(this.voxels[i]), this.voxelSize * 0.8f);
//             }
//         }
//     }

//     private Vector3 CalculateMaxBuoyancyForce()
//     {
//         float objectVolume = this.rigidbody.mass  / this.density;
//         Vector3 maxBuoyancyForce = viscosity * objectVolume * -Physics.gravity;

//         return maxBuoyancyForce;
//     }

//     private Vector3[] CutIntoVoxels()
//     {
//         Quaternion initialRotation = this.transform.rotation;
//         this.transform.rotation = Quaternion.identity;

//         Bounds bounds = this.collider.bounds;
//         this.voxelSize.x = bounds.size.x * this.normalizedVoxelSize;
//         this.voxelSize.y = bounds.size.y * this.normalizedVoxelSize;
//         this.voxelSize.z = bounds.size.z * this.normalizedVoxelSize;
//         int voxelsCountForEachAxis = Mathf.RoundToInt(1f / this.normalizedVoxelSize);
//         List<Vector3> voxels = new List<Vector3>(voxelsCountForEachAxis * voxelsCountForEachAxis * voxelsCountForEachAxis);

//         for (int i = 0; i < voxelsCountForEachAxis; i++)
//         {
//             for (int j = 0; j < voxelsCountForEachAxis; j++)
//             {
//                 for (int k = 0; k < voxelsCountForEachAxis; k++)
//                 {
//                     float pX = bounds.min.x + this.voxelSize.x * (0.5f + i);
//                     float pY = bounds.min.y + this.voxelSize.y * (0.5f + j);
//                     float pZ = bounds.min.z + this.voxelSize.z * (0.5f + k);

//                     Vector3 point = new Vector3(pX, pY, pZ);
//                     if (IsPointInsideCollider(point, this.collider, ref bounds))
//                     {
//                         voxels.Add(this.transform.InverseTransformPoint(point));
//                     }
//                 }
//             }
//         }

//         this.transform.rotation = initialRotation;

//         return voxels.ToArray();
//     }
// }





[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bouyancy : MonoBehaviour {
	public Transform water;
	public float buoyancyForce = 20f;
	public float viscosity = 20f;
    public float depthForce = 20f;
    private Rigidbody rigidBody;
	private Collider collider;

    private Vector3 centroid = Vector3.zero;
    private float submergedVolume = 0f;
    private Vector3 oldCenter;
    private Vector3 newCenter;

    private MeshFilter mesh;


    //	private float raycastSize;ß	// Use this for initialization
    void Start ()
	{
		rigidBody = GetComponent<Rigidbody> ();
		collider = GetComponent<Collider> ();
		mesh = GetComponent<MeshFilter> ();
        oldCenter = transform.position;
        newCenter = transform.position;;

//		rigidBody.AddForce (Vector3.left *1000f);
	}

	// Update is called once per frame
    private void Update()
    {
        Vector3 size = mesh.mesh.bounds.size;
        Vector3 extents = collider.bounds.extents;
        // float width = Mathf.Max(Mathf.Max(extents.x, extents.y), extents.z)*1.5f;
        //30f;//Mathf.Max (Mathf.Max (size.x, size.y), size.z) * 1.5f;

        List<Vector3> colliderPositions = new List<Vector3>();
        var min = mesh.mesh.bounds.min;
        var max = mesh.mesh.bounds.max;
        AddBBToList(colliderPositions, new Vector3(min.x, min.y, min.z));
        AddBBToList(colliderPositions, new Vector3(min.x, min.y, max.z));
        AddBBToList(colliderPositions, new Vector3(min.x, max.y, min.z));
        AddBBToList(colliderPositions, new Vector3(min.x, max.y, max.z));

        AddBBToList(colliderPositions, new Vector3(max.x, min.y, min.z));
        AddBBToList(colliderPositions, new Vector3(max.x, min.y, max.z));
        AddBBToList(colliderPositions, new Vector3(max.x, max.y, min.z));
        AddBBToList(colliderPositions, new Vector3(max.x, max.y, max.z));

        List<Vector3> positionsUnfiltered = new List<Vector3>();
        var p1 = transform.TransformPoint(new Vector3(min.x, min.y, min.z));
        var p2 = transform.TransformPoint(new Vector3(min.x, min.y, max.z));
        var p3 = transform.TransformPoint(new Vector3(min.x, max.y, min.z));
        var p4 = transform.TransformPoint(new Vector3(min.x, max.y, max.z));

        var p5 = transform.TransformPoint(new Vector3(max.x, min.y, min.z));
        var p6 = transform.TransformPoint(new Vector3(max.x, min.y, max.z));
        var p7 = transform.TransformPoint(new Vector3(max.x, max.y, min.z));
        var p8 = transform.TransformPoint(new Vector3(max.x, max.y, max.z));
        positionsUnfiltered.Add(p1);
        positionsUnfiltered.Add(p2);
        positionsUnfiltered.Add(p3);
        positionsUnfiltered.Add(p4);
        positionsUnfiltered.Add(p5);
        positionsUnfiltered.Add(p6);
        positionsUnfiltered.Add(p7);
        positionsUnfiltered.Add(p8);

        // float density = rigidBody.mass/(size.x * size.y * size.z);
        var width = (p5 - p1).magnitude;
        var height = (p3 - p1).magnitude;
        var length = (p2 - p1).magnitude;

        Debug.DrawLine(p1, p3, Color.blue);
        Debug.DrawLine(p1, p5, Color.blue);
        Debug.DrawLine(p1, p2, Color.blue);

        // Debug.DrawLine(max, min, Color.blue);
        var volume = (width * height * length);
        float density = Mathf.Abs(rigidBody.mass/volume);

        if (colliderPositions.Count > 0)
        {
            newCenter = new Vector3();
            foreach (Vector3 vec in colliderPositions)
            {
                    Debug.DrawLine(vec, vec + Vector3.up * 1f, Color.green);
                newCenter += vec;
            }
            // Debug.Log(newCenter);
            newCenter /= colliderPositions.Count;
            // Debug.Log(newCenter);
            Vector3 centre = Vector3.Lerp(oldCenter, newCenter, 0.3f);

            //Volume dependant
            submergedVolume = FindAABBVolume(colliderPositions);

            volume = FindAABBVolume(positionsUnfiltered);
            density = Mathf.Abs(rigidBody.mass/volume);
            // Debug.DrawLine(centre, centre + Vector3.up * buoyancyForce * 0.1f, Color.blue);
            // rigidBody.AddForceAtPosition(Vector3.up * buoyancyForce, centre);
            // rigidBody.AddForceAtPosition(Vector3.up * buoyancyForce, transform.position);


                // Debug.Log(dimensions);
                Debug.Log(density);
                Debug.Log(volume);
                Debug.Log(submergedVolume);
                // Debug.Log(force);

            // Vector3 force = -Physics.gravity * density * volume * buoyancyForce;
            Vector3 force = -Physics.gravity * density * submergedVolume * buoyancyForce;


            Debug.DrawLine(newCenter, newCenter + force * 2f, Color.blue);
            rigidBody.AddForceAtPosition(force, newCenter);


            //Viscocity
            Vector3 velocityDir = rigidBody.velocity.normalized;
            Vector3 v1 = Vector3.Cross(velocityDir, Vector3.right);
            Vector3 v2 = Vector3.Cross(velocityDir, v1);
            v1.Normalize();
            v2.Normalize();

            Vector3 pointOutFront = transform.position + (velocityDir * 1f);
            Debug.DrawLine(pointOutFront, pointOutFront + v1 * 2f, Color.yellow);
            Debug.DrawLine(pointOutFront, pointOutFront + v2 * 2f, Color.yellow);
            Debug.DrawLine(transform.position, pointOutFront, Color.magenta);

            int layerMask = 1 << 4;


            RaycastHit hit;
            for (float x = -width; x < width; x += width/15f)
            {
                for (float y = -width; y < width; y += width/15f)
                {
                    Vector3 start = pointOutFront + v1 * x + v2 * y;
					// Debug.DrawLine(start, start - velocityDir * 25f, Color.magenta);
                    // Debug.Log(start);
                    // Debug.DrawLine(start, start - velocityDir * 10f, Color.red);
                    if (Physics.Raycast(start, -velocityDir, out hit, 5f))
                    {

                        if(hit.transform == transform && hit.point.y <= water.position.y)
                        {
                            // Debug.Log("Hit!");
                            Vector3 hitVector = hit.point - start;
                            // Debug.DrawLine(hit.point, start, Color.red);
                            Debug.DrawLine(hit.point, hit.point + hitVector * viscosity, Color.red);
                            rigidBody.AddForceAtPosition(hitVector * viscosity, hit.point);
                            // rigidBody.AddForceAtPosition(-rigidBody.velocity * viscosity, hit.point);
                        }
                    }
                }
            }
        }
        else
        {
            // buoyancyForceVector3 = Vector3.down * depthForce;
            // buoyancyForceVector3 = Vector3.Lerp(buoyancyForceVector3, Vector3.zero, Time.deltaTime);
            // Debug.DrawLine(transform.position, transform.position + buoyancyForceVector3, Color.red);
            // rigidBody.AddForceAtPosition(buoyancyForceVector3, transform.position);
        }

    }

    void AddBBToList(List<Vector3> boxColliderPositions, Vector3 pos)
	{
		Vector3 worldPos = transform.TransformPoint(pos);
		if (worldPos.y < water.position.y)
		{
			boxColliderPositions.Add(worldPos);
		}
	}

    float FindAABBVolume(List<Vector3> points)
    {
        float volume = 0;
        Vector3 min = Vector3.one;
        Vector3 max = Vector3.one;

        for (int i = 0; i< points.Count;i++)
        {
            if (i == 0)
            {
                min = points[0];
                max = points[0];
            }
            else
            {
                min = findMin(min, points[i]);
                max = findMax(max, points[i]);
            }
        }

        var p1 = transform.TransformPoint(new Vector3(min.x, min.y, min.z));
        var p2 = transform.TransformPoint(new Vector3(min.x, min.y, max.z));
        var p3 = transform.TransformPoint(new Vector3(min.x, max.y, min.z));
        var p5 = transform.TransformPoint(new Vector3(max.x, min.y, min.z));

        var width = (p5 - p1).magnitude;
        var height = (p3 - p1).magnitude;
        var length = (p2 - p1).magnitude;

        // volume = (min.x - max.x);
        // volume *= (min.y - max.y);
        // volume *= (min.z - max.z);

        volume = width * height * length;
        return volume;
    }



    Vector3 findMin(Vector3 a, Vector3 b)
    {
        Vector3 vec = a;
        //Debug.Log(vec);
        //Debug.Log(a);
        //Debug.Log(b);
        vec.x = Mathf.Min(a.x, b.x);
        vec.y = Mathf.Min(a.y, b.y);
        vec.z = Mathf.Min(a.z, b.z);
        //Debug.Log(vec);
        return vec;
    }
    Vector3 findMax(Vector3 a, Vector3 b)
    {
        Vector3 vec = a;
        vec.x = Mathf.Max(a.x, b.x);
        vec.y = Mathf.Max(a.y, b.y);
        vec.z = Mathf.Max(a.z, b.z);
        return vec;
    }
}
