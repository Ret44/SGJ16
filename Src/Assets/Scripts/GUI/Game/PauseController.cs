using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour
{
	#region Variables

	#endregion Variables

	#region Monobehaviour Methods

	#endregion Monobehaviour Methods

	#region Methods

	public void ClickResume()
	{
		GameManager gameManagerIstance = GameManager.Instance;
		if(gameManagerIstance != null)
		{
			gameManagerIstance.ChangeGameState(GameManager.GameState.GS_GAME);
		}
    }
	public void ClickRestart()
	{
		bool reload = true;
		if (reload)
		{
			GameManager gameManagerIstance = GameManager.Instance;
			if (gameManagerIstance != null)
			{
				gameManagerIstance.RestartLevel();
			}
		} else {
			GameLord.Instance.LoadLevel(GameLord.Instance.CurrentLevelIndex);
		}
	}
	public void ClickMenu()
	{
		GameLord gameLordInstance = GameLord.Instance;
		if (gameLordInstance != null)
		{
			gameLordInstance.BackToMenu();
		}
	}

	#endregion Methods
}
