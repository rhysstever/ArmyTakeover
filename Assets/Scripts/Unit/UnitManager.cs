using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitManager : MonoBehaviour
{
	// Set in inspector
	public GameObject playerUnitsParent;
	public GameObject enemyUnitsParent;
	public GameObject unitPrefab;

	// Set at Start()
	public List<GameObject> selectedBases;
    
    // Start is called before the first frame update
    void Start()
    {
        selectedBases = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetMouseButtonDown(0))
			SelectBase(true);
		else if(Input.GetMouseButtonDown(1))
			SelectBase(false);

		CheckUnits();
	}

	/// <summary>
	/// Selects a base when the player clicks on one
	/// </summary>
	/// <param name="isLeftMouseClicked">Whether the left mouse button was clicked</param>
	private void SelectBase(bool isLeftMouseClicked) 
    {
		Camera currentCam = Camera.main;

		// Creates ray
		Ray ray = currentCam.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit;

		// Creates a layerMask to include all but the UI layer
		int layerMask = 1 << 5;
		layerMask = ~layerMask;

		// If the ray interects with something in the scene that is not UI
		if(Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask)) {	
			// Ensures anything that is clicked on is the parent
			GameObject selectedGameObj = GetParentObject(rayHit.transform.gameObject);

			// Ensures the selected gameObject is a base
			if(selectedGameObj.tag != "Base") {
				selectedBases.Clear();
				return;
			}

			switch(selectedGameObj.GetComponent<Base>().team) {
				case Team.Blue:
					if(isLeftMouseClicked) {
						if(!selectedBases.Contains(selectedGameObj))
							selectedBases.Add(selectedGameObj);
					} else {
						goto default;
					}
					break;
				default:
					if(selectedBases != null)
						SendPlayerUnits(selectedGameObj);
					break;
			}
		}
	}

	/// <summary>
	/// Given a gameObject, returns its parent, or itself if it is the parent
	/// </summary>
	/// <param name="gameObj">The gameObject that is either a parent or a child</param>
	/// <returns>The parent of the given gameObject</returns>
	private GameObject GetParentObject(GameObject gameObj) 
	{
		while(gameObj.transform.parent != null
			&& gameObj.transform.parent.tag != "Collection") {
			gameObj = gameObj.transform.parent.gameObject;
		}

		return gameObj;
	}

	private void SendPlayerUnits(GameObject target) 
    {
		// Send units from each selected base
		foreach(GameObject selectedBase in selectedBases) {
			selectedBase.GetComponent<Base>().SendUnits(target, playerUnitsParent, unitPrefab, GetComponent<MaterialManager>().matLog[Team.Blue]);
		}
		// Clears the list of selected bases
		selectedBases.Clear();
	}

	public void SendEnemyUnits(GameObject sourceBase, GameObject target) 
	{
		Team team = sourceBase.GetComponent<Base>().team;
		sourceBase.GetComponent<Base>().SendUnits(target, enemyUnitsParent, unitPrefab, GetComponent<MaterialManager>().matLog[team]);
	}

	private void CheckUnits() 
	{
		// Checks all units
		for(int i = 0; i < playerUnitsParent.transform.childCount + enemyUnitsParent.transform.childCount; i++) {
			// Finds the current unit
			GameObject unit;
			if(i >= playerUnitsParent.transform.childCount)
				unit = enemyUnitsParent.transform.GetChild(i - playerUnitsParent.transform.childCount).gameObject;
			else
				unit = playerUnitsParent.transform.GetChild(i).gameObject;

			// Checks if the unit is colliding with its target or an enemy unit
			if(unit.GetComponent<Unit>().isColliding) {
				if(unit.GetComponent<Unit>().reachedTarget) {
					GameObject target = unit.GetComponent<Unit>().target;
					target.GetComponent<Base>().ChangeUnitCount(unit.GetComponent<Unit>().team, unit.GetComponent<Unit>().worth);
				}
				Destroy(unit);
			}
		}
	}
}
