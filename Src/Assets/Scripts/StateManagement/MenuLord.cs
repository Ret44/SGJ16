using UnityEngine;
using System.Collections;

public class MenuLord : MonoBehaviour
{
	#region Variables

	private static MenuLord _instance = null;
	public static MenuLord Instance { get { return MenuLord._instance; } }

	public enum MenuState
	{
		MS_MAIN = 0,
		MS_SINGLE = 1,
		MS_OPTIONS = 2,
		MS_CREDITS = 3,
		MS_EXIT_PROMPT = 4,

		MS_COUNT,
		MS_NONE
	}

	private MenuState _currentMenuState = MenuState.MS_NONE;

	[HideInInspector]
	[SerializeField]
	private int _menuStateCount = 0;

	[System.Serializable]
	public struct MenuStateInfo
	{
		public MenuState state;
		public GameObject panelGO;
	}

	[SerializeField]
	private MenuStateInfo[] _menuStateInfos = null;

	#endregion Variables

	#region Monobehaviour Methods

	void OnValidate()
	{
		Validate();
	}

	void Awake()
	{
		InitializeMenuLord();
	}

	void OnEnable()
	{
		ResetMenuLord();
	}

	#endregion Monobehaviour Methods

	#region Methods

	private void Validate()
	{
		MenuStateInfo[] oldMenuStateInfos = _menuStateInfos;
		int oldMenuStateInfoCount = oldMenuStateInfos != null ? oldMenuStateInfos.Length : 0;
		_menuStateCount = (int)MenuState.MS_COUNT;

		if(_menuStateCount != oldMenuStateInfoCount)
		{
			_menuStateInfos = new MenuStateInfo[_menuStateCount];
		}

		for (int i = 0; i < _menuStateCount; ++i)
		{
			if (i < _menuStateCount)
			{
				_menuStateInfos[i] = oldMenuStateInfos[i];
			}
			_menuStateInfos[i].state = (MenuState)i;
		}

	}

	private void InitializeMenuLord()
	{
		MenuLord._instance = this;

		_currentMenuState = MenuState.MS_MAIN;
	}

	private void ResetMenuLord()
	{
		if (_menuStateInfos != null)
		{
			for (int i = 0; i < _menuStateCount; ++i)
			{
				_menuStateInfos[i].panelGO.SetActive(_menuStateInfos[i].state == _currentMenuState);
			}
		}
	}

	public void ChangeMenuState(MenuState newMenuState)
	{
		SetPanelActive(_currentMenuState, false);
		SetPanelActive(newMenuState, true);
		_currentMenuState = newMenuState;
	}

	private void SetPanelActive(MenuState menuState, bool active)
	{
		int index = (int)menuState;
		if(index >= 0 && index < _menuStateCount)
		{
			_menuStateInfos[index].panelGO.SetActive(active);
		}
	}

	#endregion Methods
}
