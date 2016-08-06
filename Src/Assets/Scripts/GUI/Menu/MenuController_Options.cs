using UnityEngine;
using System.Collections;

public class MenuController_Options : MonoBehaviour
{
	#region Variables

	#endregion Variables

	#region Monobehaviour Methods

	#endregion Monobehaviour Methods

	#region Methods

	public void ClickBack()
	{
		MenuLord menuLordInstance = MenuLord.Instance;
		if (menuLordInstance != null)
		{
			menuLordInstance.ChangeMenuState(MenuLord.MenuState.MS_MAIN);
		}
	}

	#endregion Methods
}
