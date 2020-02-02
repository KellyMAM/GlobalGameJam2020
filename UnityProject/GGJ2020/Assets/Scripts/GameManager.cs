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

		private int _currentStartStoryBoardIndex = 0;
		private int _currentEndStoryBoardIndex = 0;

		private void Awake()
		{
			_attractor.StartButton.onClick.AddListener(FadeInStoryboard);
			_attractor.StartStoryboardButton.onClick.AddListener(NextStartStoryBoard);
			_attractor.EndStoryboardButton.onClick.AddListener(NextEndStoryBoard);
		}

		private void FadeInStoryboard()
		{
			_currentStartStoryBoardIndex = 0;
			_attractor.NextStartStoryboard(_currentStartStoryBoardIndex);
			_attractor.TurnOffAttractor();
		}

		private void NextStartStoryBoard()
		{
			if (_currentStartStoryBoardIndex == _attractor.StartStoryboardSprites.Length - 1)
			{
				StartGame();
			}
			else
			{
				_currentStartStoryBoardIndex++;
				_attractor.NextStartStoryboard(_currentStartStoryBoardIndex);
			}
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

			_attractor.TurnOffMain();
		}

		private void NextEndStoryBoard()
		{
			if (_currentEndStoryBoardIndex == _attractor.EndStoryboardSprites.Length - 1)
			{
				EndGame();
			}
			else
			{
				_currentEndStoryBoardIndex++;
				_attractor.NextEndStoryboard(_currentEndStoryBoardIndex);
			}
		}

		private void PlayPayOff()
		{
			if (_currentLevel != null)
			{
				Destroy(_currentLevel.gameObject);
				_currentLevel = null;
			}

			_attractor.TurnOnMain();
			_attractor.TurnOffAttractor();

			_attractor.TurnOffStartStoryboard();

			Debug.Log("Well done you have completed the game");

		}

		private void EndGame()
		{
			Debug.Log("End of the game Jack");
			_attractor.TurnOnAttractor();
			_attractor.TurnOnStartStoryboard();
		}
	}
}