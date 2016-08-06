using UnityEngine;
using System.Collections;

public class RotatingImage : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private RectTransform _rectTransform = null;

	private float _rotatingSpeed = 90.0f;

	#endregion Variables

	#region Monobehaviour Methods

	void Update()
	{
		if(_rectTransform != null)
		{
			Quaternion quat = _rectTransform.localRotation;
			quat *= Quaternion.Euler(0.0f, 0.0f, _rotatingSpeed * Time.unscaledDeltaTime);
			_rectTransform.localRotation = quat;
		}
	}

	#endregion Monobehaviour Methods

	#region Methods

	#endregion Methods
}
