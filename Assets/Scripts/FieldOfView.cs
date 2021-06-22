using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;

	public float findDelay = 0.2f;

	public LayerMask targetMask;
	public LayerMask emittorMask;
	public LayerMask obstacleMask;

	public SentryBotAI sentry;

	public List<Transform> visibleTargets = new List<Transform>();

	void Start()
	{
		sentry = sentry.GetComponent<SentryBotAI> ();
		viewRadius = sentry.xRadius;
		StartCoroutine (FindTargetsWithDelay (findDelay));
	}

	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	void FindVisibleTargets()
	{
		visibleTargets.Clear ();
		Collider[] targetsInRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInRadius.Length; i++) {
			Transform target = targetsInRadius [i].transform;
			Vector3 directionTarget = (target.position - transform.position).normalized;

			if (Vector3.Angle (transform.forward, directionTarget) < viewAngle / 2) {
				float distToTarget = Vector3.Distance (transform.position, target.position);

				if (!Physics.Raycast (transform.position, directionTarget, distToTarget, obstacleMask)) {
					visibleTargets.Add (target);
					sentry.inSight = true;
					sentry.RedLightOfTerror.SetActive (true);
				} else {
					sentry.inSight = false;
					sentry.RedLightOfTerror.SetActive (false);
				}
			}


		}
		Collider[] emittorInRadius = Physics.OverlapSphere (transform.position, viewRadius, emittorMask);

		for (int i = 0; i < emittorInRadius.Length; i++) {
			Transform emittor = emittorInRadius [i].transform;
			Vector3 dirToEmittor = (emittor.position - transform.position).normalized;

			if (Vector3.Angle (transform.forward, dirToEmittor) < viewAngle / 2) {				
				float distToTarget = Vector3.Distance (transform.position, emittor.position);

				if (!Physics.Raycast (transform.position, dirToEmittor, distToTarget, obstacleMask)) {
					visibleTargets.Add (emittor);
				}
			}
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

}
