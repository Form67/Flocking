using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderFlock : MonoBehaviour {
    public GameObject[] flock;
    public Vector2 strength1;
    public Vector2 strength2;
    public Vector2 strength3;

	public float seperationConstant;
	public float maxSpeed; //Could be changed to maxAcceleration
	// Use this for initialization
	void Start () {
        flock = GameObject.FindGameObjectsWithTag("BOID");
	}
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject a in flock) {
            strength1 = avoid_collisions(a);
            strength2 = match_velocity(a);
            strength3 = flock_to_center(a);
            a.GetComponent<Rigidbody2D>().velocity = a.GetComponent<Rigidbody2D>().velocity + strength1 + strength2 + strength3;
        }

	}
    public Vector2 avoid_collisions(GameObject b) {
		float strength = Mathf.Min (seperationConstant / Vector3.Distance (transform.position, b.transform.position), maxSpeed);

		return strength * (transform.position - b.transform.position).normalized;

    }
    public Vector2 match_velocity(GameObject b)
    {
        return new Vector2(0, 0);

    }
    public Vector2 flock_to_center(GameObject b)
    {
        return new Vector2(0, 0);

    }
    public void coneCheck(GameObject b) {
        
    }
    public void collisionPrediction(GameObject b)
    {
        
    }
}
