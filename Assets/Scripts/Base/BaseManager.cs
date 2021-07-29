using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    // Set in inspector
    public GameObject basesParent;
    public int playerUnitStartingCount;
    public int playerUnitCap;
    public float playerTrainingRate;
    public int neutralUnitCap;
    public float neutralTrainingRate;
    public int enemyUnitStartingCount;
    public int enemyUnitCap;
    public float enemyTrainingRate;

    // Set at Start()


    // Start is called before the first frame update
    void Start() {
        // Assign the correct material to each created base
        for(int i = 0; i < basesParent.transform.childCount; i++) {
            GameObject baseInScene = basesParent.transform.GetChild(i).gameObject;
            Team baseTeam = baseInScene.GetComponent<Base>().team;
            int startingUnitCount = 0;
            switch(baseTeam) {
                case Team.Blue:
                    startingUnitCount = playerUnitStartingCount;
                    break;
                case Team.None:
                    startingUnitCount = neutralUnitCap;
                    break;
                default:
                    startingUnitCount = enemyUnitStartingCount;
                    break;
            }
            SetBaseValues(baseInScene, baseTeam, startingUnitCount);
        }
    }

    // Update is called once per frame
    void Update() {
        for(int i = 0; i < basesParent.transform.childCount; i++) {
            GameObject baseInScene = basesParent.transform.GetChild(i).gameObject;

            switch(baseInScene.GetComponent<Base>().team) {
                case Team.Red:
                    if(baseInScene.GetComponent<EnemyBase>() != null)
                        SendEnemyUnits(baseInScene);
                    break;
            }

            if(baseInScene.GetComponent<Base>().switchingTeams) {
                Team baseTeam = baseInScene.GetComponent<Base>().team;
                SetBaseValues(baseInScene, baseTeam);
                baseInScene.GetComponent<Base>().switchingTeams = false;
            }
        }
    }

    private void SetBaseValues(GameObject baseInScene, Team team, int unitCount) 
    {
        baseInScene.GetComponent<Base>().unitCount = unitCount;
        SetBaseValues(baseInScene, team);
    }

    private void SetBaseValues(GameObject baseInScene, Team team) {
        // Set building-wide values, no matter which team its on
        baseInScene.GetComponentInChildren<MeshRenderer>().material = GetComponent<MaterialManager>().matLog[team].Primary;
        if((team == Team.Blue || team == Team.None)
            && baseInScene.GetComponent<EnemyBase>() != null)
            Destroy(baseInScene.GetComponent<EnemyBase>());

        // Set team-specific values
        switch(team) {
            // Player base
            case Team.Blue:
                baseInScene.GetComponent<Base>().unitCap = playerUnitCap;
                baseInScene.GetComponent<Base>().trainRate = playerTrainingRate;
                break;
            // Unowned base
            case Team.None:
                baseInScene.GetComponent<Base>().unitCap = neutralUnitCap;
                baseInScene.GetComponent<Base>().trainRate = neutralTrainingRate;
                break;
            // Enemy base
            default:
                baseInScene.GetComponent<Base>().unitCap = enemyUnitCap;
                baseInScene.GetComponent<Base>().trainRate = enemyTrainingRate;
                baseInScene.AddComponent<EnemyBase>();
                break;
        }
    }

    private void SendEnemyUnits(GameObject enemyBase) 
    {
        // If the enemy base has at least half as many units, it will send units
        if(enemyBase.GetComponent<Base>().unitCount * 2 >= enemyBase.GetComponent<Base>().unitCap) {
            GameObject targetBase = FindTargetBase(enemyBase);
            if(targetBase != null)
                GetComponent<UnitManager>().SendEnemyUnits(enemyBase, targetBase);
        }
    }

    private GameObject FindTargetBase(GameObject enemyBase) 
    {
        GameObject closestNonEnemyBase = null;
        for(int i = 0; i < basesParent.transform.childCount; i++) {
            if(basesParent.transform.GetChild(i).gameObject.GetComponent<Base>().team 
                != enemyBase.GetComponent<Base>().team) {
                if(closestNonEnemyBase == null)
                    closestNonEnemyBase = basesParent.transform.GetChild(i).gameObject;
                else if(Vector3.Distance(enemyBase.transform.position, basesParent.transform.GetChild(i).position)
                    < Vector3.Distance(enemyBase.transform.position, closestNonEnemyBase.transform.position))
                    closestNonEnemyBase = basesParent.transform.GetChild(i).gameObject;
            }
        }

        return closestNonEnemyBase;
	}
}