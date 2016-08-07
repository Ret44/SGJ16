using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Bounds
{
	#region Variables

	[SerializeField]
	private Vector3[] _boundries;
	public Vector3[] Boundries { get { return _boundries; } }

	[SerializeField]
	private Vector3 _size;
	public Vector3 Size { get { return _size; } }

	#endregion Variables

	#region Methods

	public void Validate()
	{
		Vector3[] oldBounds = _boundries;
		int oldBoundsCount = _boundries != null ? _boundries.Length : 0;
		if(oldBoundsCount != 2)
		{
			_boundries = new Vector3[2];
		}

		for(int i = 0;i < 2;++i)
		{
			if(i < oldBoundsCount)
			{
				_boundries[i] = oldBounds[i];
			} else {
				_boundries[i] = Vector3.zero;
			}
		}

		_size = _boundries[0] - _boundries[1];
		for(int i = 0;i < 3;++i)
		{
			_size[i] = Mathf.Abs( _size[i] );
		}
	}

	public void DrawGizmos(Color color)
	{
		Vector3 center = Vector3.Lerp(_boundries[0], _boundries[1], 0.5f);
		Gizmos.color = color;
		Gizmos.DrawWireCube(center, _size);
	}

	public Vector3 GetRandomPointInBounds()
	{
		Vector3 result = Vector3.zero;

		for(int i = 0;i < 3; ++i)
		{
			result[i] = UnityEngine.Random.Range(_boundries[0][i], _boundries[1][i]);
		}		

		return result;
	}

	#endregion Methods
}
