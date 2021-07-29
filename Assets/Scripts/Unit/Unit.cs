using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    GameObject worthText;

    public Team team;
    public GameObject target;
    public int worth;

    // Set at Start()
    public bool isColliding;
    public bool reachedTarget;

    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
        reachedTarget = false;
    }

    // Update is called once per frame
    void Update() {
        // Updates worth text with value and rotates it to the camera
        worthText.GetComponent<Text>().text = worth.ToString();
        worthText.transform.parent.LookAt(Camera.main.transform);
    }

    private void OnCollisionEnter(Collision collision) {
        // The unit is colliding if it hits its target builidng or another unit
        if(collision.gameObject == target) {
            isColliding = true;
            reachedTarget = true;
		}
        else if(collision.gameObject.tag == "Unit"
            && collision.gameObject.GetComponent<Unit>().team != team)
            isColliding = true;
    }
}
