using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team
{
    None,
    Blue, 
    Red
}

public class Base : MonoBehaviour
{
    // Set in inspector
    public Team team;

    [SerializeField]
    GameObject unitCountText;

    // Set at Start()
    public int unitCount;   // Set in BaseManager
    public int unitCap;     // Set in BaseManager
    public float trainRate; // Set in BaseManager
    private float timer;
    public bool switchingTeams;
    private bool isOverCap;

    // Start is called before the first frame update
    void Start()
    {
        unitCountText.transform.parent.LookAt(Camera.main.transform);
        timer = 0.0f;
        switchingTeams = false;
        isOverCap = false;
    }

	// Update is called once per frame
	void Update() 
    {
        unitCountText.GetComponent<Text>().text = unitCount.ToString();
        if(isOverCap)
            unitCountText.GetComponent<Text>().text += "!";
    }

    void FixedUpdate() {
        timer += Time.deltaTime;

        // If there is room for a unit
        if(unitCount < unitCap) {
            isOverCap = false;
            // Adds a unit and resets the timer
            if(timer >= trainRate) {
                unitCount++;
                timer = 0.0f;
            }
        }
        // If there are too many units
        else if(unitCount > unitCap) {
            isOverCap = true;
            // Removes a unit at twice as slow as the building would 
            // normally train a unit and resets the timer
            if(timer >= trainRate * 2) {
                unitCount--;
                timer = 0.0f;
            }
        }
        // The base is at its max count of units
        else {
            isOverCap = false;
            timer = 0.0f;
        }
    }

    public void ChangeUnitCount(Team unitTeam, int amountChangedBy) {
        // If the units are from a different team, subtract the unit count instead
        if(unitTeam != team)
            amountChangedBy *= -1;
        unitCount += amountChangedBy;

        if(unitCount < 0)
            ChangeTeam(unitTeam);
    }

    public void SendUnits(GameObject target, GameObject parent, GameObject unitPrefab, MaterialObj mats) 
    {
        GameObject newUnit = Instantiate(unitPrefab, transform.position, Quaternion.identity, parent.transform);

        // Set starting position
        Vector3 pos = unitPrefab.transform.position;
        pos.x = transform.position.x;
        pos.z = transform.position.z;
        newUnit.transform.position = pos;

        // Set Unit-specific variables
        newUnit.GetComponent<Unit>().team = team;
        newUnit.GetComponent<Unit>().worth = unitCount / 2;
        newUnit.GetComponent<Unit>().target = target;

        // Set materials
        newUnit.transform.Find("base").GetComponent<MeshRenderer>().material = mats.Primary;
        newUnit.transform.Find("base").transform.Find("face").GetComponent<MeshRenderer>().material = mats.Secondary;

        // Zero out base's unit count
        unitCount -= unitCount / 2;
    }
    
    public void ChangeTeam(Team newTeam) 
    {
        team = newTeam;
        unitCount *= -1;
        switchingTeams = true;
    }
}
