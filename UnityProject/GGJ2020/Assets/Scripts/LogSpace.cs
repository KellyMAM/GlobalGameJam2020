using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
	public class LogSpace : MonoBehaviour
	{
		[HideInInspector]
		public LogSpaceState CurrentState = LogSpaceState.Empty;


		public void SpaceFilled()
		{
			this.gameObject.SetActive(false);
		}
	}
}