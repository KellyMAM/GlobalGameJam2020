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
		private Animator _mainAnimator;
		[SerializeField]
		public Button StartButton;

		[Space]

		[SerializeField]
		private Animator _attractAnim;
		[SerializeField]
		public Button StartStoryboardButton;
		[SerializeField]
		public Sprite[] StartStoryboardSprites;
		[SerializeField]
		private Image StartStoryboardImage;

		[Space]

		[SerializeField]
		private Animator _startStoryboardAnim;
		[SerializeField]
		public Button EndStoryboardButton;
		[SerializeField]
		public Sprite[] EndStoryboardSprites;
		[SerializeField]
		private Image EndStoryboardImage;


		private void Awake()
		{
			_mainAnimator = GetComponent<Animator>();
		}

		public void TurnOnMain()
		{
			_mainAnimator.ResetTrigger("TurnOn");
			_mainAnimator.SetTrigger("TurnOn");
		}

		public void TurnOffMain()
		{
			_mainAnimator.ResetTrigger("TurnOff");
			_mainAnimator.SetTrigger("TurnOff");
		}



		public void TurnOffAttractor()
		{
			_attractAnim.ResetTrigger("TurnOff");
			_attractAnim.SetTrigger("TurnOff");
		}

		public void TurnOnAttractor()
		{
			_attractAnim.ResetTrigger("TurnOnImmediate");
			_attractAnim.SetTrigger("TurnOnImmediate");
		}

		public void NextStartStoryboard(int index)
		{
			StartStoryboardImage.sprite = StartStoryboardSprites[index];
		}




		public void TurnOffStartStoryboard()
		{
			_startStoryboardAnim.ResetTrigger("TurnOffImmediate");
			_startStoryboardAnim.SetTrigger("TurnOffImmediate");
		}

		public void TurnOnStartStoryboard()
		{
			_startStoryboardAnim.ResetTrigger("TurnOnImmediate");
			_startStoryboardAnim.SetTrigger("TurnOnImmediate");
		}

		public void NextEndStoryboard(int index)
		{
			EndStoryboardImage.sprite = EndStoryboardSprites[index];
		}
	}
}