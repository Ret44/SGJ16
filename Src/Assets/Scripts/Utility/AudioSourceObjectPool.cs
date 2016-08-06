using UnityEngine;
using System.Collections;

public class AudioSourceObjectPool : MonoBehaviour
{
	#region Variables

	private Transform _parent = null;

	private AudioSource[] _pool = null;
	public AudioSource[] Pool { get { return _pool; } }
	private int _size = 0;
	public int Size { get { return _size; } }

	private int _nextIndexToUse = 0;

	#endregion Variables

	#region Methods

	public AudioSourceObjectPool(Transform parent, int initialSize)
	{
		_parent = parent;
		_size = initialSize;
		_pool = new AudioSource[_size];
		for (int i = 0; i < _size; ++i)
		{
			GameObject tmpGameObject = new GameObject( string.Format("Source_{0}", i) );
			_pool[i] = tmpGameObject.AddComponent<AudioSource>();
            _pool[i].transform.parent = _parent;
		}
	}

	public AudioSource GetPooledObject()
	{
		AudioSource result = _pool[_nextIndexToUse];

		_nextIndexToUse = (_nextIndexToUse + 1) % _size;

		return result;
	}

	public void StopAllSources()
	{
		for (int i = 0; i < _size; ++i)
		{
			_pool[i].Stop();
			_pool[i].transform.parent = _parent;
		}
		_nextIndexToUse = 0;
	}


	#endregion Methods
}
