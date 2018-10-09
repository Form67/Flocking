using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderFlock : MonoBehaviour {
    public GameObject[] flock;
	Vector2 instantaneousVelocity;

	public float seperationConstant;
	public float closeEnoughDistance;
	public float maxSpeed; //Could be changed to maxAcceleration
	// Use this for initialization
	void Start () {
        flock = GameObject.FindGameObjectsWithTag("BOID");
		instantaneousVelocity = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject a in flock) {
            Vector2 strength1 = avoid_collisions(a);
            Vector2 strength2 = match_velocity(a);
            Vector2 strength3 = flock_to_center(a);
			Rigidbody2D rb = a.GetComponent<Rigidbody2D> ();

			rb.velocity = a.GetComponent<Rigidbody2D>().velocity + strength1 + strength2 + strength3;
			if (rb.velocity.magnitude > maxSpeed) {
				rb.velocity = maxSpeed * rb.velocity.normalized;
			}
        }
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePosition.z = 0;
		instantaneousVelocity = mousePosition - transform.position;
		transform.position = mousePosition;

	}
    public Vector2 avoid_collisions(GameObject b) {
		Vector3 seperation = Vector2.zero;
		foreach (GameObject a in flock){
			
			if (a != b) {
				float strength = Mathf.Min (seperationConstant / Vector3.Distance (a.transform.position, b.transform.position), maxSpeed);
				seperation += strength * (b.transform.position - a.transform.position).normalized;
			}
		}
		
		float strengthFromLeader = Mathf.Min (seperationConstant / Vector3.Distance (transform.position, b.transform.position), maxSpeed);
		seperation += strengthFromLeader * (b.transform.position - transform.position).normalized;
		return new Vector2(seperation.x, seperation.y);

    }
    public Vector2 match_velocity(GameObject b)
    {
		

		return instantaneousVelocity;

    }
    public Vector2 flock_to_center(GameObject b)
    {
		
		return (transform.position - b.transform.position);

    }
    public void coneCheck(GameObject b) {
        
    }
    public void collisionPrediction(GameObject b)
    {
        
    }
}
