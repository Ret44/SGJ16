using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudController : MonoBehaviour
{
	#region Variables

	private GameLord _gameLord = null;
	private HudLord _hudLord = null;

	[SerializeField]
	private Text _scoreLabel = null;

	#endregion Variables

	#region Monobehaviour Methods

	void Awake()
	{
		InitHudController();
    }

	void OnEnable()
	{
		GameManager.OnScoreChanged += HandleScoreChanged;
    }

	void OnDisable()
	{
		GameManager.OnScoreChanged -= HandleScoreChanged;
	}

	#endregion Monobehaviour Methods

	#region Methods

	private void InitHudController()
	{
		_gameLord = GameLord.Instance;
		_hudLord = HudLord.Instance;
	}

	public void ClickPause()
	{
		GameManager instance = GameManager.Instance;
		if(instance != null)
		{
			instance.ChangeGameState(GameManager.GameState.GS_PAUSE);
		}
    }

	private void HandleScoreChanged(int score)
	{
		if(_scoreLabel != null)
		{
			_scoreLabel.text = string.Format("{0}",score);
		}
	}

	#endregion Methods
}
