using UnityEngine;
using System.Collections;

public class ComponentBasedObjectPool<T> where T : MonoBehaviour
{
	#region Variables

	private T _prefab = null;
	private Transform _parent = null;

	private T[] _pool = null;
	public T[] Pool { get { return _pool; } }
	private int _size = 0;
	public int Size { get { return _size; } }

	private int _nextIndexToUse = 0;

	#endregion Variables

	#region Methods

	public ComponentBasedObjectPool(T prefab, Transform parent, int initialSize)
	{
		_prefab = prefab;
		_parent = parent;
		_size = initialSize;
		_pool = new T[_size];
		for(int i = 0;i < _size;++i)
		{
			_pool[i] = GameObject.Instantiate<T>(_prefab);
			_pool[i].transform.parent = _parent;
			_pool[i].gameObject.SetActive(false);
		}
	}

	public T GetPooledObject()
	{
		T result = null;

		for (int i = 0; i < _size; ++i, _nextIndexToUse = (_nextIndexToUse + 1) % _size)
		{
			if (!_pool[_nextIndexToUse].gameObject.activeSelf)
			{
				result = _pool[_nextIndexToUse];
				break;
			}
		}

		if (result == null)
		{
			T[] oldPool = _pool;
			_pool = new T[_size + 1];
			for (int i = 0; i < _size; ++i)
			{
				_pool[i] = oldPool[i];
			}
			_pool[_size] = GameObject.Instantiate<T>(_prefab);
			_pool[_size].transform.parent = _parent;
			_pool[_size].gameObject.SetActive(false);
			result = _pool[_size];
			++_size;
		}

		return result;
	}

	public void DisableAllObjects()
	{
		for (int i = 0; i < _size; ++i)
		{
			_pool[i].gameObject.SetActive(false);
		}
	}


	#endregion Methods
}
