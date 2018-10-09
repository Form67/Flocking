using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlock : MonoBehaviour {
	public int flockSize;
	public GameObject flockPrefab;
	public float initialSeperation;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < flockSize; i++){
			GameObject flockInstance = Instantiate (flockPrefab);
			Vector3 position = Vector3.zero;
			position.x = i * initialSeperation;
			flockInstance.transform.position = position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
