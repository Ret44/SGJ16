using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController_Credits : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private Text _creditsText = null;

	[SerializeField]
	private TextAsset _creditsTextFile = null;

	#endregion Variables

	#region Monobehaviour Methods

	void OnEnable()
	{
		ResetMenuController_Credits();
    }

	#endregion Monobehaviour Methods

	#region Methods

	private void ResetMenuController_Credits()
	{
		if(_creditsText != null && _creditsTextFile != null)
		{
			_creditsText.text = _creditsTextFile.text;
		}
	}

	public void ClickBack()
	{
		MenuLord menuLordInstance = MenuLord.Instance;
		if(menuLordInstance != null)
		{
			menuLordInstance.ChangeMenuState(MenuLord.MenuState.MS_MAIN);
		}
	}

	#endregion Methods
}
