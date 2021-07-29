using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
	// Set in inspector
	#region Materials
	public Material grayPrim;
    public Material bluePrim;
    public Material blueSec;
    public Material redPrim;
    public Material redSec;
	#endregion

	// Set at Start()
	public Dictionary<Team, MaterialObj> matLog;
    
    // Start is called before the first frame update
    void Start()
    {
        matLog = new Dictionary<Team, MaterialObj>();
        matLog.Add(Team.None, new MaterialObj(Team.None, grayPrim, null));
        matLog.Add(Team.Blue, new MaterialObj(Team.Blue, bluePrim, blueSec));
        matLog.Add(Team.Red, new MaterialObj(Team.Red, redPrim, redSec));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
