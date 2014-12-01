//The "View" for the game camera.

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace strange.examples.strangerobots.game
{
	public class CameraView : View
	{
		[Inject]
		public IScreenUtil screenUtil{ get; set; }

		private Vector3 startPos;
		private Vector3 endPos;

		private Quaternion startRot;
		private Quaternion endRot;


		private float startTime = 0;
		private float journeyDistance = 0;

		public float journeySeconds = 2f;
		public Vector3 destRot = Vector3.forward;


		public void GoToPosition(Vector3 dest)
		{
			StartCoroutine (gotoPosition(dest));
		}
		
		void OnEnable()
		{
			startPos = endPos = transform.position;
			startRot = endRot = transform.rotation;
		}
		
		void FixedUpdate()
		{
			if (journeyDistance > 0) {
				float fracJourney = (Time.time - startTime) / journeySeconds;
				transform.position = Vector3.Slerp(startPos, endPos, fracJourney);
				transform.rotation = Quaternion.Slerp(startRot, endRot, fracJourney);

				if (Vector3.Distance(startPos, endPos) < .01f) {
					journeyDistance = 0;
				}
				//SetObliqueness (0, fracJourney);
			}


		}

		IEnumerator gotoPosition(Vector3 dest) {
			yield return new WaitForSeconds(1.0f);
			startPos = transform.position;
			endPos = dest;
			endRot = Quaternion.Euler(destRot);
			journeyDistance = Vector3.Distance(startPos, endPos);
			startTime = Time.time;
		}

		void SetObliqueness(float horizObl, float vertObl) {
			Matrix4x4 mat = camera.projectionMatrix;
			mat[0, 2] = horizObl;
			mat[1, 2] = vertObl;
			camera.projectionMatrix = mat;
		}
	}
}

