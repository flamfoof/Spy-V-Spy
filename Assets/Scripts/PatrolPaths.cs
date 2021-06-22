using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPaths : MonoBehaviour {
	public List<PatrolPaths> paths;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		//Gizmos.

		foreach (var path in paths) {
			if (path != null)
				Gizmos.DrawLine (transform.position, path.transform.position);
		}
	}
}
