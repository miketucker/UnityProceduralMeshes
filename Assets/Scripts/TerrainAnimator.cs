using UnityEngine;
using System.Collections;

public class TerrainAnimator : MonoBehaviour {

	public Terrain targetTerrain;
	public float noiseSpeed = 0.01f;
	public float noiseFrequency = 0.1f;
	
	private float currentNoiseOffset = 0f;
	private TerrainData terrainData;
	private int resolution;
	private float[,] cacheHeights;
	private ImprovedPerlin perlin;

	// Use this for initialization
	void Start () {
	
		if(targetTerrain == null) targetTerrain = Terrain.activeTerrain;
		if(targetTerrain == null) Debug.LogError("Please add a terrain object to the scene.");

		terrainData = targetTerrain.terrainData;
		resolution = terrainData.heightmapResolution;
		cacheHeights = new float[resolution,resolution];

		perlin = new ImprovedPerlin();

	}
	
	// Update is called once per frame
	void Update () {
	
		currentNoiseOffset += Time.deltaTime * noiseSpeed;

		for(int x = 0; x < resolution; x++){
			for(int y = 0; y < resolution; y++){
				cacheHeights[x,y] = perlin.Noise( x * noiseFrequency , y * noiseFrequency, currentNoiseOffset);
			}
		}

		terrainData.SetHeights(0,0,cacheHeights);

	}
}
