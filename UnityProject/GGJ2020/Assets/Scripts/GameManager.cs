using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
	public class GameManager : MonoBehaviour
	{
 		[SerializeField]
		private Attractor _attractor;
		[SerializeField]
		private Level _levelPrefab;

		private Level _currentLevel;

		private void Awake()
		{
			_attractor.StartButton.onClick.AddListener(FadeInStoryboard);
			_attractor.StoryboardButton.onClick.AddListener(StartGame);
		}

		private void TurnOnAttractor()
		{
			_attractor.TurnOnAttractor();
		}

		private void TurnOffAttractor()
		{
			_attractor.TurnOffAttractor();
		}

		private void FadeInStoryboard()
		{
			_attractor.TurnOffHomeScreen();
		}

		private void StartGame()
		{
			Debug.Log("Starting game");
			if (_currentLevel != null)
			{
				Destroy(_currentLevel.gameObject);
				_currentLevel = null;
			}
			_currentLevel = Instantiate<GameObject>(_levelPrefab.gameObject, this.transform).GetComponent<Level>();
			_currentLevel.transform.position = Vector3.zero;
			_currentLevel.DamCollector.OnAllLogSpacesFilled += PlayPayOff;

			TurnOffAttractor();
		}

		private void EndGame()
		{
			TurnOnAttractor();
		}

		private void PlayPayOff()
		{
			Debug.Log("Well done you have completed the game");

			EndGame();
		}
	}
}