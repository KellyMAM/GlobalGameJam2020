using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
	public enum LogState
	{
		InField,
		moving,
		Placed
	}
	public enum LogSpaceState
	{
		Empty,
		Filled,
		Filling
	}

	public class DamCollector : MonoBehaviour
	{
		[SerializeField]
		private List<LogSpace> _LogSpaces = new List<LogSpace>();

		private List<Log> _logsPlaced = new List<Log>();

		private float _rangeCheck = 3f;
		private float _duration = 2f;

		public event Action OnAllLogSpacesFilled = delegate { };
		public event Action<Log> OnLogPlaced = delegate { };

		[SerializeField]
		private AnimationCurve Curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Logs" && other.gameObject.GetComponentInParent<Rigidbody>() != null)
			{
				Log log = other.GetComponentInParent<Log>();
				if (log != null && log.CurrentState == LogState.InField)
				{
					log.CurrentState = LogState.moving;

					LogSpace logSpace = LogSpaceTransform();
					logSpace.CurrentState = LogSpaceState.Filling;

					StartCoroutine(LerpTo(logSpace, log));
				}
			}
		}

		private LogSpace LogSpaceTransform()
		{
			LogSpace logSpace = null;

			for (int i = 0; i < _LogSpaces.Count; i++)
			{
				if (_LogSpaces[i].CurrentState == LogSpaceState.Empty)
				{
					return _LogSpaces[i];
				}
			}

			return logSpace;
		}

		private bool DistanceCheck(Transform target, Transform item)
		{
			bool inRange = false;

			float dis = Vector3.Distance(target.position, item.position);
			float angleLeft = Vector3.Angle(target.position, item.position);
			if (dis < _rangeCheck && angleLeft < _rangeCheck)
			{
				inRange = true;
			}

			return inRange;
		}

		private IEnumerator LerpTo(LogSpace target, Log log)
		{
			float elapsedTime = 0f;
			Debug.Log("Starting Lerp");
			log.LogPlaced();

			while (elapsedTime < _duration)
			{
				float value = Curve.Evaluate((elapsedTime / _duration));

				//Position
				log.transform.position = Vector3.Lerp(log.transform.position, target.transform.position, value);

				//Rotation
				Vector3 rotation = Vector3.Lerp(log.transform.rotation.eulerAngles, target.transform.rotation.eulerAngles, value);
				rotation = new Vector3(0, rotation.y, 0);
				log.transform.eulerAngles = rotation;


				elapsedTime += Time.deltaTime;
				yield return null;
			}

			//Locking values
			log.transform.position = target.transform.position;
			log.transform.rotation = target.transform.rotation;

			target.CurrentState = LogSpaceState.Filled;
			log.CurrentState = LogState.Placed;

			_logsPlaced.Add(log);

			OnLogPlaced(log);
			Debug.Log("Completed Lerp");

			target.SpaceFilled();

			LogSpacesFilledCheck();

			yield return null;
		}

		private void LogSpacesFilledCheck()
		{
			Debug.Log("Checking for filled spaces");

			for (int i = 0; i < _LogSpaces.Count; i++)
			{
				if (_LogSpaces[i].CurrentState == LogSpaceState.Empty || _LogSpaces[i].CurrentState == LogSpaceState.Filling)
				{
					return;
				}
			}

			Debug.Log("All Log Spaces have been Filled!");

			OnAllLogSpacesFilled();
		}

		[ContextMenu("TestTheEnd")]
		private void TestTheEnd()
		{
			OnAllLogSpacesFilled();
		}
	}
}