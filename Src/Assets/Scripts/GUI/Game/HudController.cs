using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour
{
	#region Variables

	private GameLord _gameLord = null;
	private HudLord _hudLord = null;

	#endregion Variables

	#region Monobehaviour Methods

	void Awake()
	{
		InitHudController();
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

	#endregion Methods
}
