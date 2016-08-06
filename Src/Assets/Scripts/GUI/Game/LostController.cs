using UnityEngine;
using System.Collections;

public class LostController : MonoBehaviour
{
	#region Variables

	#endregion Variables

	#region Monobehaviour Methods

	#endregion Monobehaviour Methods

	#region Methods

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
