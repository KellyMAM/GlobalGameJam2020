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
		private Animator _animatorAttractor;

		[SerializeField]
		private Animator _animatorHome;

		[SerializeField]
		public Button StartButton;

		[SerializeField]
		public Button StoryboardButton;

		[SerializeField]
		public Sprite[] StoryboardSprites;

		[SerializeField]
		private Image StoryboardImage;

		private void Awake()
		{
			_animatorAttractor = GetComponent<Animator>();
		}

		public void TurnOnAttractor()
		{
			_animatorAttractor.ResetTrigger("TurnOn");
			_animatorAttractor.SetTrigger("TurnOn");
		}

		public void TurnOffAttractor()
		{
			_animatorAttractor.ResetTrigger("TurnOff");
			_animatorAttractor.SetTrigger("TurnOff");
		}

		public void TurnOffHomeScreen()
		{
			_animatorHome.ResetTrigger("TurnOff");
			_animatorHome.SetTrigger("TurnOff");
		}

		public void TurnOnHomeScreen()
		{
			_animatorHome.ResetTrigger("TurnOnImmediate");
			_animatorHome.SetTrigger("TurnOnImmediate");
		}

		public void NextStoryboard(int index)
		{
			StoryboardImage.sprite = StoryboardSprites[index];
		}
	}
}