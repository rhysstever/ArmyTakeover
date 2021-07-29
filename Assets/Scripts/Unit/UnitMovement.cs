using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    // Set in inspector
    public float moveSpeed;

    // Set at Start()


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update() 
    {
        if(GetComponent<Unit>().target != null)
            LookAtTarget();
        Ground();
    }

	void FixedUpdate() {
        Move();
	}

	private void Move() {
        // Get direction to target
        Vector3 distVec = GetComponent<Unit>().target.transform.position - transform.position;
        distVec.Normalize();
        distVec.y = 0.0f;
        // Scale it based on the move speed
        distVec /= 100;
        distVec *= moveSpeed;
        // Apply it to the unit's position
        transform.position += distVec;
    }

	private void LookAtTarget() {
        transform.LookAt(GetComponent<Unit>().target.transform);
        Quaternion newQuat = transform.rotation;
        newQuat.x = 0.0f;
        newQuat.z = 0.0f;
        transform.rotation = newQuat;
    }

    private void Ground() {
        float height = transform.localScale.y / 2;
        Vector3 newPos = new Vector3(
            transform.position.x,
            height, 
            transform.position.z);
        transform.position = newPos;
	}
}
