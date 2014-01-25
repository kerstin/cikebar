﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// http://www.fraghalt.de/blog/unity3d-endless-runner-tutorial/
public class BackgroundBehaviour : MonoBehaviour {
	public Transform prefab;
	public Transform obstacle;
	private Vector2 position;		

	private LinkedList<Transform> roads = new LinkedList<Transform>();
	private LinkedList<Transform> obstacles = new LinkedList<Transform>();
	private float posX = 0.0f;
	private int numberOfRoads = 7;
	public static float speed = 8.0f;
	private Transform lastRoad;
	public float gameLength = 100f;
	public float probability = 0.3f;
	public float granularity = 3f;
	private float gameProgress = 0.0f;
	// Use this for initialization
	void Start () {
		obstacleGenerator (gameLength, probability, granularity);
		
		// Init the scene with some road-pieces
		for(int i=0;i < numberOfRoads;i++) {
			for(int j = 0; j < 2; j++)
			{
				Transform road = Instantiate(prefab) as Transform;
				road.Translate(  j * road.renderer.bounds.size.x/1.8f-road.renderer.bounds.size.x/3.6f,i * road.renderer.bounds.size.y, 0);
				roads.AddLast(road);
			}
		}
		lastRoad = roads.Last.Value;
	}

	// Use this for initialization
	void Update () {
				Transform firstRoad = roads.First.Value;
		
				// Create a new road if the first one is not 
				// in sight anymore and destroy the first one
				if (firstRoad.localPosition.y < -15f) {
						roads.Remove (firstRoad);
						Destroy (firstRoad.gameObject);
						newTile();
				}
		
		// Move the available roads along the z-axis
		foreach(Transform road in roads) {
			road.Translate( 0f,-speed * Time.deltaTime,0f);      
		}
		foreach(Transform obstac in obstacles) {
			obstac.Translate( 0f,-speed * Time.deltaTime,0f);      
		}

		gameProgress += speed * Time.deltaTime;
		Debug.Log (gameProgress);
		if (gameProgress > gameLength)
						restart ();
	}
	void restart()
	{
		foreach(Transform obstac in obstacles) {
			obstac.Translate( 0f,gameProgress,0f);      
		}
		gameProgress = 0.0f;
	}
	// 0..100
	// probability 0..1 if smaller 1 always
	// granularity stepsize (how often could there be an object: between 1-5 should be ok
	void obstacleGenerator(float fieldLength, float probability, float granularity)
	{
		for(float i = 8.0f; i < fieldLength; i+=granularity)
		{
			if(Random.Range(0.0f,1.0f)< probability)
			{
				Transform obstacle1 = Instantiate (obstacle) as Transform;
				obstacle1.Translate(Random.Range(-2.5f, 2.5f), i, 0);
				obstacles.AddLast (obstacle1);
			}
		}
	}
	void newTile()
	{

		for (int j = 0; j < 2; j++) {
			Transform newRoad = Instantiate (prefab) as Transform;
			newRoad.Translate (j * lastRoad.renderer.bounds.size.x/1.8f - lastRoad.renderer.bounds.size.x / 3.6f, lastRoad.localPosition.y + lastRoad.renderer.bounds.size.y, 0);
			roads.AddLast (newRoad);
		}
		lastRoad = roads.Last.Value;
		
		
	}
}	