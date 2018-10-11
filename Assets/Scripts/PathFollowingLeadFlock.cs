using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingLeadFlock : LeaderFlock {

	public GameObject pathObject;
	public float fractionOfLineLookAhead;
	Path path;

	Rigidbody2D rbody;

	void Start(){
		flock = GetComponent<SpawnFlock> ().flockList.ToArray();
		path = pathObject.GetComponent<Path> ();
		rbody = GetComponent<Rigidbody2D> ();
	}

	void Update(){
		manageFlock ();
		PathFollowing (path);
		instantaneousVelocity = rbody.velocity;
	}

	void PathFollowing(Path p){
		Vector3 closestPoint = p.linePoints[0];
		int closestIndex = 0;

		for (int i = 1; i < p.linePoints.Count; ++i) {
			if (Vector3.Distance (p.linePoints [i], transform.position) < Vector3.Distance (closestPoint, transform.position)) {
				closestPoint = p.linePoints [i];
				closestIndex = i;
			}
		}

		int targetIndex = closestIndex +(int) (fractionOfLineLookAhead * (float)p.linePoints.Count); 
		if (targetIndex >= p.linePoints.Count) {
			targetIndex = p.linePoints.Count - 1;
		}

		DynamicSeek (p.linePoints [targetIndex]);
		transform.eulerAngles = new Vector3 (0, 0, GetNewOrientation () * Mathf.Rad2Deg);
	}

	void DynamicSeek(Vector3 target)
	{
		Vector2 linear_acc = target - transform.position;
		if (linear_acc.magnitude > maxAcceleration) {
			linear_acc = linear_acc.normalized * maxAcceleration;
		}

		rbody.velocity += linear_acc;


		if (rbody.velocity.magnitude > maxSpeed) {
			rbody.velocity = rbody.velocity.normalized * maxSpeed;
		}
	}

	float GetNewOrientation() {
		if (rbody.velocity.magnitude > 0f) {
			return Mathf.Atan2 (-rbody.velocity.x, rbody.velocity.y);
		}
		return transform.eulerAngles.z * Mathf.Deg2Rad;
	}
}
