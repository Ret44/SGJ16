﻿using UnityEngine;
using System.Collections;

public class HudLord : MonoBehaviour
{
	#region Variables

	private static HudLord _instance = null;
	public static HudLord Instance {  get { return _instance; } }

	private GameManager.GameState _currentHudState = GameManager.GameState.GS_NONE;
	public GameManager.GameState CurrentHudState { get { return _currentHudState; } }

	[System.Serializable]
	public struct HudStateInfo
	{
		public GameManager.GameState state;
		public GameObject panelGO;
	}

	[SerializeField]
	private HudStateInfo[] _hudStateInfos = null;
	[HideInInspector]
	[SerializeField]
	private int _hudStateCount = 0;

	private bool _eventsConnected = false;

	#endregion Variables

	#region Monobehaviour Methods

	void OnValidate()
	{
		Validate();
	}
	void Awake()
	{
		InitHudLord();
	}
	void OnEnable()
	{
		ResetHudLord();

		ConnectEvents(true);
	}
	void OnDisable()
	{
		ConnectEvents(false);
	}
	#endregion Monobehaviour Methods

	#region Methods

	private void Validate()
	{
		HudStateInfo[] oldHudStateInfos = _hudStateInfos;
		int oldHudStateInfoCount = oldHudStateInfos != null ? oldHudStateInfos.Length : 0;
		_hudStateCount = (int)GameManager.GameState.GS_COUNT;

		if(oldHudStateInfoCount != _hudStateCount)
		{
			_hudStateInfos = new HudStateInfo[_hudStateCount];
		}
		for (int i = 0; i < _hudStateCount; ++i)
		{
			if(i < oldHudStateInfoCount)
			{
				_hudStateInfos[i] = oldHudStateInfos[i];
			}
			_hudStateInfos[i].state = (GameManager.GameState)i;
		}
	}

	private void InitHudLord()
	{
		ConnectEvents(true);

		HudLord._instance = this;
	}

	private void ResetHudLord()
	{
		GameManager gameManagerInstace = GameManager.Instance;
		if(gameManagerInstace != null)
		{
			_currentHudState = gameManagerInstace.CurrentGameState;
		} else {
			_currentHudState = GameManager.GameState.GS_NONE;
		}
		for(int i = 0;i < _hudStateCount;++i)
		{
			_hudStateInfos[i].panelGO.SetActive(_hudStateInfos[i].state == _currentHudState);
		}
	}

	public void ChangeHudState(GameManager.GameState newHudState)
	{
		SetPanelActive(_currentHudState, false);
		SetPanelActive(newHudState, true);
		_currentHudState = newHudState;
	}

	public void SetPanelActive(GameManager.GameState hudState, bool active)
	{
		int index = (int)hudState;
		if(index >= 0 && index < _hudStateCount)
		{
			_hudStateInfos[index].panelGO.SetActive(active);
		}
	}

	private void HandleGameState(GameManager.GameState gameState)
	{
		ChangeHudState(gameState);
	}

	private void ConnectEvents(bool state)
	{
		if (state && !_eventsConnected)
		{
			GameManager.OnGameStateChanged += HandleGameState;
		}
		if (!state && _eventsConnected)
		{
			GameManager.OnGameStateChanged -= HandleGameState;
		}

		_eventsConnected = state;
	}

	#endregion Methods
}
