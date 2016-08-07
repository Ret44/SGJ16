using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private Transform _transform = null;
	[SerializeField]
	private Transform _playerTransform = null;

	private float _lerpFactor = 0.1f;

	private Vector3 _boom = Vector3.zero;

	private bool _wasInitialized = false;

	private bool _eventsConnected = false;

	#endregion Variables

	#region Monobehaviour Methods

	void Awake()
	{
		InitializeCameraController();
		ConnectEvents(true);
	}
	void OnEnable()
	{
		ResetCameraController();
		ConnectEvents(true);
	}
	void OnDisable()
	{
		ConnectEvents(false);
	}
	void Update()
	{
		ProcessCameraController();
    }

	#endregion Monobehaviour Methods

	#region Methods

	void InitializeCameraController()
	{
		if(!_wasInitialized)
		{
			_boom = _transform.position - _playerTransform.position;

			_wasInitialized = true;
		}
	}

	void ResetCameraController()
	{
		InitializeCameraController();
	}

	private void ConnectEvents(bool connect)
	{
		if (!_eventsConnected && connect)
		{
			GameManager.OnGameReset += ResetCameraController;
        }
		if (_eventsConnected && !connect)
		{
			GameManager.OnGameReset -= ResetCameraController;
		}

		_eventsConnected = connect;
	}

	private void ProcessCameraController()
	{
		Vector3 targetPosition = _playerTransform.position + _boom;

		Vector3 tmpPositoon = _transform.position;

		tmpPositoon = Vector3.Lerp(tmpPositoon, targetPosition, _lerpFactor);

		_transform.position = tmpPositoon;
	}

	#endregion Methods
}
