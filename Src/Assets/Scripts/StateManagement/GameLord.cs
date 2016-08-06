using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/* //for copy-pasta
	#region Variables

	#endregion Variables

	#region Monobehaviour Methods

	#endregion Monobehaviour Methods

	#region Methods

	#endregion Methods
*/

public class GameLord : MonoBehaviour
{
	#region Variables

	private static GameLord _instance = null;
	public static GameLord Instance
	{
		get
		{
			if(_instance == null)
			{
				SceneManager.LoadScene(startSceneName, LoadSceneMode.Additive);
			}
			return GameLord._instance;
		}
	}

	public enum GameState : int
	{
		GS_MENU = 0,
		GS_GAME = 1,
		GS_LOADING = 2,

		GS_COUNT,
		GS_NONE
	}

	public enum TransitionState
	{
		TS_NONE = 0,
		TS_OUT = 1,
		TS_IN = 2
	}

	private TransitionState _currentTransitionState = TransitionState.TS_NONE;
	bool IsTransition { get { return _currentTransitionState != TransitionState.TS_NONE; } }

	private GameState _currentGameState = GameState.GS_NONE;
	private GameState _targetGameState = GameState.GS_NONE;
	public GameState CurrentGameState { get { return _currentGameState; } }
	public static System.Action<GameState> OnGameStateChanged;

	[SerializeField]
	private Image _fader = null;

	[SerializeField]
	private string[] _sceneNames = null;
	[HideInInspector]
	[SerializeField]
	private int _sceneNameCount = 0;

	[System.Serializable]
	public struct StateInfo
	{
		public GameState state;
		public GameObject panelGO;
	}

	[SerializeField]
	private StateInfo[] _stateInfos = null;
	[HideInInspector]
	[SerializeField]
	private int _stateCount = 0;

	private float _transitionTimer = 0.0f;
	private float _transitionLength = 1.0f;

	private AsyncOperation _loadingAsyncOperation = null;
	private int _targetLevelIndex = -1;
	private int _currentSceneIndex = -1;
	private string _currentSceneName = "";
	public const string startSceneName = "Start";

	#endregion Variables

	#region Monobehaviour Methods

	void OnValidate()
	{
		Validate();
    }

	void Awake()
	{
		InitializeGameLord();
    }

	void Update()
	{
		ProcessState();
		ProcessTransition();
	}

	#endregion Monobehaviour Methods

	#region Methods

	private void Validate()
	{
		_stateCount = (int)GameState.GS_COUNT;
		StateInfo[] oldPanelInfos = _stateInfos;
		int oldPanelCount = oldPanelInfos != null ? oldPanelInfos.Length : 0;

		if(_stateCount != oldPanelCount)
		{
			_stateInfos = new StateInfo[_stateCount];
		}
		for(int i = 0;i < _stateCount;++i)
		{
			if(i < oldPanelCount)
			{
				_stateInfos[i] = oldPanelInfos[i];
			}
			_stateInfos[i].state = (GameState)i;
		}
	}

	private void InitializeGameLord()
	{
		GameLord._instance = this;

		_currentTransitionState = TransitionState.TS_NONE;
		_targetGameState = GameState.GS_NONE;

		string loadedSceneName = SceneManager.GetActiveScene().name;
		if(loadedSceneName == startSceneName)
		{
			_currentGameState = GameState.GS_MENU;
		} else {
			bool foundAny = false;
			for(int i = 0;i < _sceneNameCount;++i)
			{
				if(_sceneNames[i] == loadedSceneName)
				{
					foundAny = true;
					_currentSceneIndex = i;
					_currentGameState = GameState.GS_GAME;
					break;
				}
			}
			if(!foundAny)
			{
				_currentGameState = GameState.GS_NONE;
				Debug.LogErrorFormat("Scene {0} not added to GameLord!!!", loadedSceneName);
			}
		}

		for(int i = 0;i < _stateCount;++i)
		{
			_stateInfos[i].panelGO.SetActive(_stateInfos[i].state == _currentGameState);
		}
	}

	public void LoadLevel(int index)
	{
		_targetLevelIndex = index;
		ChangeGameState(GameState.GS_LOADING);
	}

	public void BackToMenu()
	{
		ChangeGameState(GameState.GS_MENU);
	}

	private void ChangeGameState(GameState newGameState)
	{
		if(newGameState != _currentGameState)
		{
			if (_currentTransitionState == TransitionState.TS_NONE)
			{
				_targetGameState = newGameState;
				_currentTransitionState = TransitionState.TS_OUT;
			} else {

				_targetGameState = newGameState;
				if (_currentTransitionState == TransitionState.TS_OUT)
				{
					//do nth
				} else {
					_currentTransitionState = TransitionState.TS_OUT;
				}
			}
		} else{
			//it's the same state
			if(_currentTransitionState == TransitionState.TS_OUT)
			{
				_currentTransitionState = TransitionState.TS_IN;
			} else {
				// do nth
			}
		}
	}

	private void ProcessState()
	{
		switch(_currentGameState)
		{
			case GameState.GS_MENU:
				break;
			case GameState.GS_GAME:
				break;
			case GameState.GS_LOADING:
				if(_targetLevelIndex != -1)
				{
					if(_currentSceneIndex != -1)
					{
						SceneManager.UnloadScene(_sceneNames[_currentSceneIndex]);

						Resources.UnloadUnusedAssets();
					}

					_loadingAsyncOperation = SceneManager.LoadSceneAsync(_sceneNames[_targetLevelIndex], LoadSceneMode.Additive);
					_targetLevelIndex = -1;
				} else {

					if(_loadingAsyncOperation != null && _loadingAsyncOperation.isDone)
					{
						ChangeGameState(GameState.GS_GAME);
					}
				}
				break;
			case GameState.GS_NONE:
				break;
		}
	}

	private void ProcessTransition()
	{
		float deltaTime = Time.deltaTime;
		switch(_currentTransitionState)
		{
			case TransitionState.TS_NONE:
				if(_fader.gameObject.activeSelf)
				{
					_fader.gameObject.SetActive(false);
				}
				break;
			case TransitionState.TS_OUT:
				_transitionTimer += deltaTime;
				if(_transitionTimer > _transitionLength)
				{
					_transitionTimer = _transitionLength;

					SetPanelActive(_currentGameState, false);
					SetPanelActive(_targetGameState, true);
					_currentGameState = _targetGameState;
					_targetGameState = GameState.GS_NONE;

					_currentTransitionState = TransitionState.TS_IN;
				} else {
					if (!_fader.gameObject.activeSelf)
					{
						_fader.gameObject.SetActive(true);
					}
					Color faderColor = _fader.color;
					faderColor.a = Mathf.Clamp01(_transitionTimer / _transitionLength);
					_fader.color = faderColor;
				}
                break;
			case TransitionState.TS_IN:
				_transitionTimer -= deltaTime;
				if(_transitionTimer < 0.0f)
				{
					_transitionTimer = 0.0f;
					_currentTransitionState = TransitionState.TS_NONE;

					NotifyOnGameStateChanged();
                } else {
					if (!_fader.gameObject.activeSelf)
					{
						_fader.gameObject.SetActive(true);
					}
					Color faderColor = _fader.color;
					faderColor.a = Mathf.Clamp01(_transitionTimer / _transitionLength);
					_fader.color = faderColor;
				}
				break;
		}
	}

	private void SetPanelActive(GameState gameState, bool active)
	{
		int index = (int)gameState;
		if(_stateInfos != null && index >= 0 && index < _stateCount && _stateInfos[index].panelGO != null)
		{
			_stateInfos[index].panelGO.SetActive(active);
        }
	}

	private void NotifyOnGameStateChanged()
	{
		if (OnGameStateChanged != null)
		{
			OnGameStateChanged(_currentGameState);
		}
	}

	#endregion Methods
}
