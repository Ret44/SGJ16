using UnityEngine;
using System.Collections;

public class ShakyLogo : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private RectTransform[] _images = null;
	[HideInInspector]
	[SerializeField]
	int _imageCount = 0;

	private Vector2[] _directions = null;
	private Vector2[] _basePositions = null;
	private float[] _timeMultiplier = null;
	private float[] _ranges = null;

	private float _timer = 0.0f;

	#endregion Variables

	#region Monobehaviour Methods

	void OnValidate()
	{
		_imageCount = _images != null ? _images.Length : 0;
	}

	void Awake()
	{
		InitializeShakyLogo();
    }

	void Update()
	{
		ProcessShakyLogo();
    }

	#endregion Monobehaviour Methods

	#region Methods

	private void InitializeShakyLogo()
	{
		_directions = new Vector2[_imageCount];
		_basePositions = new Vector2[_imageCount];
		_timeMultiplier = new float[_imageCount];
		_ranges = new float[_imageCount];
		float deltaAngle = (Mathf.PI * 2.0f) / (float)(_imageCount+1);
		for(int i = 0;i < _imageCount;++i)
		{
			_basePositions[i] = _images[i].anchoredPosition;
			_directions[i].x = Mathf.Sin(i * deltaAngle);
			_directions[i].y = Mathf.Sin(i * deltaAngle);
			_timeMultiplier[i] = UnityEngine.Random.Range(1.0f, 3.0f);
			_ranges[i] = UnityEngine.Random.Range(5.0f, 50.0f);
		}
	}

	private void ProcessShakyLogo()
	{
		_timer += Time.unscaledDeltaTime * 2.0f;
		for (int i = 0; i < _imageCount; ++i)
		{
			_images[i].anchoredPosition = _basePositions[i] + _directions[i] * Mathf.Pow(Mathf.Sin(_timeMultiplier[i] * _timer) * _ranges[i], 1.0f );
        }
	}

	#endregion Methods
}
