using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	#region Variables

	private static GameManager _instance = null;
	public static GameManager Instance { get { return GameManager._instance; } }

	private GameLord _gameLord = null;

	public enum GameState
	{
		GS_WARM_UP = 0,
		GS_GAME = 1,
		GS_PAUSE = 2,
		GS_WAIT_LOST = 3,
		GS_LOST = 4,
		GS_WAIT_WIN = 5,
		GS_WIN = 6,

		GS_COUNT,
		GS_NONE
	}

	private GameState _currentGameState = GameState.GS_NONE;
	public GameState CurrentGameState { get { return _currentGameState; } }
	public static System.Action<GameState> OnGameStateChanged;

	private CrowdController _crowController = null;
	private Player _player = null;

	private float _stateTimer = 0.0f;

	private const float stateLength_warmUp = 1.0f;
	private const float stateLength_waitLost = 2.0f;
	private const float stateLength_waitWin = 2.0f;

	private bool _eventsConnected = false;

	#endregion Variables

	#region Monobehaviour Methods

	void Awake()
	{
		//_gameLord = GameLord.Instance;

		InitGameManager();
	}

	void OnEnable()
	{
		ConnectEvents(true);
	}
	void OnDisable()
	{
		ConnectEvents(false);
	}

	void Update()
	{
		ProcessGameStates();
	}

	#endregion Monobehaviour Methods

	#region Methods

	private void InitGameManager()
	{
		GameManager._instance = this;

		ConnectEvents(true);

		_currentGameState = GameState.GS_NONE;

		_crowController = GameObject.FindObjectOfType<CrowdController>();

		_player = GameObject.FindObjectOfType<Player>();

		ChangeGameState(GameState.GS_NONE);
    }
	
	public void ChangeGameState(GameState newGameState)
	{
		_currentGameState = newGameState;

		_crowController.ResetCrowdController();

		NotifyOnGameStateChanged();
	}

	public void RestartLevel()
	{
		bool reload = true;
		if (reload)
		{
			GameLord.Instance.LoadLevel(GameLord.Instance.CurrentLevelIndex);
		} else {
			if (_player != null)
			{
				//_player.ResetPlayer();
			}

			CrowdController.ResetAll();

			ChangeGameState(GameState.GS_WARM_UP);
		}
	}

	private void ProcessGameStates()
	{
		
		switch(_currentGameState)
		{
			case GameState.GS_WARM_UP:
				_stateTimer += Time.unscaledDeltaTime;
				if(_stateTimer > stateLength_warmUp)
				{
					ChangeGameState(GameState.GS_GAME);
				}
				break;
			case GameState.GS_GAME:
				{
					if(_player.dead)
					{
						ChangeGameState(GameState.GS_WAIT_LOST);
					} else{
						//some nice win condition
					}
				}
				break;
			case GameState.GS_PAUSE:
				break;
			case GameState.GS_WAIT_LOST:
				_stateTimer += Time.unscaledDeltaTime;
				if(_stateTimer > stateLength_waitLost)
				{
					ChangeGameState(GameState.GS_LOST);
				} else {
					GameLord.Instance.UpdateTimeScale();
				}
				break;
			case GameState.GS_LOST:
				break;
			case GameState.GS_WAIT_WIN:
				_stateTimer += Time.unscaledDeltaTime;
				if (_stateTimer > stateLength_waitLost)
				{
					ChangeGameState(GameState.GS_WIN);
				} else {
					GameLord.Instance.UpdateTimeScale();
				}
				break;
			case GameState.GS_WIN:
				break;
		}
	}

	private void NotifyOnGameStateChanged()
	{
		GameLord gameLordInstance = GameLord.Instance;
		if(gameLordInstance != null)
		{
			gameLordInstance.UpdateTimeScale();
		}

		if(GameManager.OnGameStateChanged != null)
		{
			GameManager.OnGameStateChanged(_currentGameState);
		}
	}

	private void HandleAppState(GameLord.AppState appState)
	{
		switch(appState)
		{
			case GameLord.AppState.AS_GAME:
				ChangeGameState(GameState.GS_GAME);
				break;
			default:
				ChangeGameState(GameState.GS_NONE);
				break;
		}
	}


	public void UpdateTimeScale()
	{
		switch(_currentGameState)
		{
			case GameState.GS_WARM_UP:
				Time.timeScale = 0.0f;
				break;
			case GameState.GS_GAME:
				Time.timeScale = 1.0f;
				break;
			case GameState.GS_PAUSE:
				Time.timeScale = 0.0f;
				break;
			case GameState.GS_WAIT_LOST:
				Time.timeScale = Mathf.Clamp01(1.0f - Mathf.Clamp01(_stateTimer / stateLength_waitLost));
				break;
			case GameState.GS_LOST:
				Time.timeScale = 0.0f;
				break;
			case GameState.GS_WAIT_WIN:
				Time.timeScale = Mathf.Clamp01(1.0f - Mathf.Clamp01(_stateTimer / stateLength_waitWin) );
				break;
			case GameState.GS_WIN:
				Time.timeScale = 0.0f;
				break;
			case GameState.GS_NONE:
				Time.timeScale = 0.0f;
				break;
		}
	}

	private void ConnectEvents(bool state)
	{
		if(state && !_eventsConnected)
		{
			Debug.LogWarning("Connect");
			GameLord.OnGameStateChanged += HandleAppState;
		}
		if(!state && _eventsConnected)
		{
			GameLord.OnGameStateChanged -= HandleAppState;
		}

		_eventsConnected = state;
	}

	#endregion Methods
}
