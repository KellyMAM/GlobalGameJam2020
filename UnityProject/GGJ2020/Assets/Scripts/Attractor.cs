using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
	public class Attractor : MonoBehaviour
	{
		[SerializeField]
		private Animator _animator;

		[SerializeField]
		public Button StartButton;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		public void TurnOnAttractor()
		{
			_animator.ResetTrigger("TurnOn");
			_animator.SetTrigger("TurnOn");
		}

		public void TurnOffAttractor()
		{
			_animator.ResetTrigger("TurnOff");
			_animator.SetTrigger("TurnOff");
		}
	}
}