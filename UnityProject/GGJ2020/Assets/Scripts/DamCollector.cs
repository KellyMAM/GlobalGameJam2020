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
		private List<Coroutine> _lerpToCoroutines = new List<Coroutine>();

		private float _rangeCheck = 1f;
		private float _duration = 5f;

		public event Action OnAllLogSpacesFilled = delegate { };
		public event Action<Log> OnLogPlaced = delegate { };

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Logs" && other.gameObject.GetComponent<Rigidbody>() != null)
			{
				Log log = other.GetComponent<Log>();
				if (log != null && log.CurrentState == LogState.InField)
				{
					log.CurrentState = LogState.moving;

					LogSpace logSpace = LogSpaceTransform();
					logSpace.CurrentState = LogSpaceState.Filling;

					_lerpToCoroutines.Add(StartCoroutine(LerpTo(_lerpToCoroutines.Count, logSpace, log)));
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

		/*		private bool DistanceCheck(Transform target, Transform item)
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
		*/
		private IEnumerator LerpTo(int index, LogSpace target, Log log)
		{
			float elapsedTime = 0f;
			Debug.Log("Starting Lerp");
			while (elapsedTime < _duration)
			{
				log.transform.position = Vector3.Lerp(log.transform.position, target.transform.position, (elapsedTime / _duration));
				log.transform.rotation = Quaternion.Euler(Vector3.Lerp(log.transform.rotation.eulerAngles, target.transform.rotation.eulerAngles, (elapsedTime / _duration)));
				yield return null;
			}

			target.CurrentState = LogSpaceState.Filled;
			log.CurrentState = LogState.Placed;

			_logsPlaced.Add(log);
			log.LogPlaced();

			OnLogPlaced(log);
			Debug.Log("Completed Lerp");

			target.SpaceFilled();

			_lerpToCoroutines[index] = null;
			_lerpToCoroutines.RemoveAt(index);

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
	}
}