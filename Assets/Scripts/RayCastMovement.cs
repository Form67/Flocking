using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastMovement : MonoBehaviour {
    //Using arbitration to deal with corner trap
    public GameObject[] path;
    public float arbitrationCooldown;
    public float avoidDistance;
    public int arbitrationWinner;
    public float maxArbitrationCooldown;
    public float raycastAngle;
    public Vector2 target;
    public float raycastDistance;
    public float max_acc;
    public float curr_speed;
    public float max_speed;
    public float linear_acc;
    public bool arrived;
    public int currentIndex;
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

        float zRotation = Mathf.Atan2(-GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * zRotation);
        

    }
    public void Seek() 
        {
            linear_acc = Vector3.Distance(transform.position, target);
            //go to object plus where it will be in a second (pursue);
            var magnitude = target- new Vector2(transform.position.x, transform.position.y);
            magnitude = magnitude.normalized;
            transform.up = magnitude;
            if (linear_acc > max_acc)
            {
                linear_acc = max_acc;
            }
            curr_speed += linear_acc;
            if (curr_speed > max_speed)
            {
                curr_speed = max_speed;
            }
            GetComponent<Rigidbody2D>().velocity = (magnitude * curr_speed);

        }

    public void Pathfind()
    {
        if (!arrived)// seeks to each node on path
        {
            linear_acc = Vector3.Distance(transform.position, path[currentIndex].transform.position);
            var magnitude = -transform.position + path[currentIndex].transform.position;
            magnitude = magnitude.normalized;
            if (linear_acc > max_acc)
            {
                linear_acc = max_acc;
            }
            curr_speed += linear_acc;
            if (curr_speed > max_speed)
            {
                curr_speed = max_speed;
            }
            GetComponent<Rigidbody2D>().velocity = (magnitude * curr_speed);
            if (Vector3.Distance(this.transform.position, path[currentIndex].transform.position) < .5f)
            {
                if (currentIndex < path.Length - 1)
                {
                    currentIndex++;
                    transform.up = path[currentIndex].transform.position - transform.position;
                }
                else
                {
                    arrived = true;
                }
            }
        }
    }
}
