using UnityEngine;

public class MaterialObj
{
	#region Fields
	private Team team;
	private Material primaryMat;
	private Material secondaryMat;
	#endregion

	#region Properties
	public Team Team { get { return team; } }
	public Material Primary { get { return primaryMat; } }
	public Material Secondary { get { return secondaryMat; } }
	#endregion

	#region Constructor
	public MaterialObj(Team team, Material primaryMat, Material secondaryMat) {
		this.team = team;
		this.primaryMat = primaryMat;
		this.secondaryMat = secondaryMat;
	}
	#endregion

	#region Methods
	#endregion
}
