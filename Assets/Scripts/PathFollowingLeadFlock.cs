using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingLeadFlock : LeaderFlock {

	public GameObject pathObject;
	public float fractionOfLineLookAhead;
	Path path;

	Rigidbody2D rbody;

	public bool isConeCheck;
	public bool isCollisionPrediction;


	List<Vector3> originalPositions;

	void Start(){
		flock = GetComponent<SpawnFlock> ().flockList.ToArray();
		path = pathObject.GetComponent<Path> ();
		rbody = GetComponent<Rigidbody2D> ();
		isConeCheck = true;
		isCollisionPrediction = false;
		originalPositions = new List<Vector3> ();
		originalPositions.Add (transform.position);
		foreach (GameObject f in flock) {
			originalPositions.Add (f.transform.position);
		}
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

	public void resetToOriginalPosition(){
		transform.position = originalPositions [0];
		for (int i = 1; i < originalPositions.Count; ++i) {
			flock [i - 1].transform.position = originalPositions [i];
		}
	}

	public override void manageIndividualUnit(GameObject a){
		Rigidbody2D rb = a.GetComponent<Rigidbody2D> ();
		if (isConeCheck) {
			rb.velocity += coneCheck (a);
		}
		if (isCollisionPrediction) {
			rb.velocity += collisionPrediction (a);
		}
		base.manageIndividualUnit (a);


	}

	public Vector2 coneCheck(GameObject b) {
		return Vector2.zero;
	}

	public Vector2 collisionPrediction(GameObject b) {
		GameObject[] otherAgents = GameObject.FindGameObjectsWithTag ("BOID");

		GameObject closestCollidingAgent = null;
		float tClosest = 0;

		Vector2 ourVelocity = b.GetComponent<Rigidbody2D> ().velocity;

		foreach (GameObject agent in otherAgents) {
			if (Vector3.Distance (agent.transform.position, b.transform.position) < closeEnoughDistance) {
				Vector3 distanceVector = agent.transform.position - b.transform.position;
				Vector2 dp = new Vector2 (distanceVector.x, distanceVector.y);

				Vector2 theirVelocity = agent.GetComponent<Rigidbody2D> ().velocity;
				Vector2 dv = theirVelocity - ourVelocity;

				float t = -(Vector2.Dot (dp, dv) / Mathf.Pow (dv.magnitude, 2));
				if (t > tClosest) {
					tClosest = t;
					closestCollidingAgent = agent;
				}
			} 
		}
		if (closestCollidingAgent != null) {

			Vector3 ourVelocity3D = ourVelocity;
			Vector3 predictedPosition = b.transform.position + tClosest * ourVelocity3D;
			Vector3 theirVelocity3D = closestCollidingAgent.GetComponent<Rigidbody2D> ().velocity;
			Vector3 targetPredictedPosition = closestCollidingAgent.transform.position + tClosest * theirVelocity3D;
		}

		return Vector2.zero;
	}
}
