using UnityEngine;
using System.Collections;

public class MenuController_Single : MonoBehaviour
{
	#region Variables

	#endregion Variables

	#region Monobehaviour Methods

	#endregion Monobehaviour Methods

	#region Methods

	public void ClickPlayLevel(int index)
	{
		GameLord gameLordInstance = GameLord.Instance;
		if (gameLordInstance != null)
		{
			gameLordInstance.LoadLevel(index);
		}
	}

	public void ClickBack()
	{
		MenuLord menuLordInstance = MenuLord.Instance;
		if (menuLordInstance != null)
		{
			menuLordInstance.ChangeMenuState(MenuLord.MenuState.MS_SINGLE);
		}
	}

	#endregion Methods
}
