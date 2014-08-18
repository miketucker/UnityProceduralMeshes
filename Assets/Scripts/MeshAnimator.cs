using UnityEngine;
using System.Collections;

public class MeshAnimator : MonoBehaviour {

	public Material material;
	public int resolution = 64;
	public float noiseSpeed = 0.1f;
	public float noiseFrequency = 0.1f;
	public float noiseAmplitude = 0.1f;


	private Mesh planeMesh;
	private ImprovedPerlin perlin;
	private float currentNoiseOffset = 0f;
	private Vector3[] cacheVertices;


	// Use this for initialization
	void Start () {
		
		// generate the mesh
		planeMesh = PlaneHelper.CreatePlane(resolution,resolution);
		
		// add this whenever you'll be updating the mesh frequently
		planeMesh.MarkDynamic();

		// add these components for our GameObject to be able to display meshes
		MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();

		// assign our new mesh to this gameobject
		// sharedMesh means the data will be a pointer instead of copied
		mf.sharedMesh = planeMesh;
		mr.material = material;

		// cache the vertex array so we won't need to allocate each frame
		cacheVertices = planeMesh.vertices;

		perlin = new ImprovedPerlin();
	}
	
	// Update is called once per frame
	void Update () {

		currentNoiseOffset += Time.deltaTime * noiseSpeed;

		int i = 0;
		for(int y = 0; y < resolution; y++){
			for(int x = 0; x < resolution; x++){
				cacheVertices[i].y = perlin.Noise( x * noiseFrequency , y * noiseFrequency, currentNoiseOffset) * noiseAmplitude;
				i++;
			}
			
		}

		// replace the old vertices with the new ones
		planeMesh.vertices = cacheVertices;

		// generate new normals so that the lighting works correctly
		planeMesh.RecalculateNormals();
	}
}
