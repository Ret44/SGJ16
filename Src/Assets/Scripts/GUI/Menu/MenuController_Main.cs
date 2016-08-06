using UnityEngine;
using System.Collections;

public class MenuController_Main : MonoBehaviour
{
	#region Variables

	#endregion Variables

	#region Monobehaviour Methods

	#endregion Monobehaviour Methods

	#region Methods

	public void ClickSingle()
	{
		MenuLord menuLordInstance = MenuLord.Instance;
		if(menuLordInstance != null)
		{
			menuLordInstance.ChangeMenuState(MenuLord.MenuState.MS_SINGLE);
		}
	}
	public void ClickOptions()
	{
		MenuLord menuLordInstance = MenuLord.Instance;
		if (menuLordInstance != null)
		{
			menuLordInstance.ChangeMenuState(MenuLord.MenuState.MS_OPTIONS);
		}
	}
	public void ClickCredits()
	{
		MenuLord menuLordInstance = MenuLord.Instance;
		if (menuLordInstance != null)
		{
			menuLordInstance.ChangeMenuState(MenuLord.MenuState.MS_CREDITS);
		}
	}
	public void ClickExit()
	{
		MenuLord menuLordInstance = MenuLord.Instance;
		if (menuLordInstance != null)
		{
			menuLordInstance.ChangeMenuState(MenuLord.MenuState.MS_EXIT_PROMPT);
		}
	}

	#endregion Methods
}
