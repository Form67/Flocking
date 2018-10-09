using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastMovement : MonoBehaviour {
    //Using arbitration to deal with corner trap
    public float arbitrationCooldown;
    public float avoidDistance;
    public int arbitrationWinner;
    public float maxArbitrationCooldown;
    public float raycastAngle;
    public Vector2 target;
    public float raycastDistance;

    
    public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (arbitrationCooldown <= 0 || arbitrationWinner == 1)
        {
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, new Vector2(raycastAngle, 1), raycastDistance);
            if (hit1.collider != null)
            {
                arbitrationCooldown = maxArbitrationCooldown;
                arbitrationWinner = 1;
                target = hit1.point + hit1.normal * avoidDistance;
            }
        }
        if (arbitrationCooldown <= 0 || arbitrationWinner == 2) {

            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(-raycastAngle, 1), raycastDistance);
            if (hit2.collider != null)
            {
                arbitrationWinner = 2;
                arbitrationCooldown = maxArbitrationCooldown;
                target = hit2.point + hit2.normal * avoidDistance;
            }
        }
        arbitrationCooldown -= Time.deltaTime;
        if (arbitrationCooldown < 0) {
            arbitrationWinner = 0;
        }
        if (arbitrationWinner == 0)
        {
            Pathfind();
        }
        else {
            Seek();
        }

    }
    public void Seek() {

    }

    public void Pathfind() {

    }
}
