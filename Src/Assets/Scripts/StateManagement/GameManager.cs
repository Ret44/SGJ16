using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	#region Variables

	private static GameManager _instance = null;
	public static GameManager Instance { get { return GameManager._instance; } }

	public enum GameState
	{
		HS_WARM_UP = 0,
		HS_GAME = 1,
		HS_PAUSE = 2,
		GS_WAIT = 3,
		HS_LOST = 4,
		HS_WIN = 5,

		HS_COUNT,
		HS_NONE
	}

	private GameState _currentGameState = GameState.HS_NONE;
	public GameState CurrentGameState { get { return _currentGameState; } }
	public static System.Action<GameState> OnGameStateChanged;

	#endregion Variables

	#region Monobehaviour Methods

	void Awake()
	{
		InitGameManager();
	}

	void OnEnable()
	{
		GameLord.OnGameStateChanged += HandleAppState;
	}
	void OnDisable()
	{
		GameLord.OnGameStateChanged -= HandleAppState;
	}

	void Update()
	{

	}

	#endregion Monobehaviour Methods

	#region Methods

	private void InitGameManager()
	{
		GameManager._instance = this;

		_currentGameState = GameState.HS_NONE;

		ChangeGameState(GameState.HS_NONE);
    }
	
	public void ChangeGameState(GameState newGameState)
	{
		_currentGameState = newGameState;
		NotifyOnGameStateChanged();
	}

	private void NotifyOnGameStateChanged()
	{
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
				ChangeGameState(GameState.HS_GAME);
				break;
			default:
				ChangeGameState(GameState.HS_NONE);
				break;
		}
	}

	#endregion Methods
}
