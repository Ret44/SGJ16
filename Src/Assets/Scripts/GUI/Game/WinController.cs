using UnityEngine;
using System.Collections;

public class WinController : MonoBehaviour
{
	#region Variables

	#endregion Variables

	#region Monobehaviour Methods

	#endregion Monobehaviour Methods

	#region Methods

	public void ClickNext()
	{
		GameLord gameLordInstance = GameLord.Instance;
		if (gameLordInstance != null)
		{
			gameLordInstance.NextLevel();
		}
	}

	public void ClickRestart()
	{
		GameManager gameManagerIstance = GameManager.Instance;
		if (gameManagerIstance != null)
		{
			gameManagerIstance.RestartLevel();
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
