using UnityEngine;
using System.Collections;

public class MemberObjectPool
{
	#region Variables

	private GameObject _prefab = null;
	private Transform _parent = null;

	private GameObject[] _pool = null;
	public GameObject[] Pool {  get { return _pool; } }
	private int _size = 0;
	public int Size { get { return _size; } }

	private int _nextIndexToUse = 0;

	#endregion Variables

	#region Methods

	public MemberObjectPool(GameObject prefab, Transform parent, int initialSize)
	{
		_prefab = prefab;
		_parent = parent;
		_size = initialSize;
	}

	public GameObject GetPooledObject()
	{
		GameObject result = null;

		for(int i = 0;i < _size;++i, _nextIndexToUse = (_nextIndexToUse + 1) % _size)
		{
			if(!_pool[_nextIndexToUse].activeSelf)
			{
				result = _pool[_nextIndexToUse];
				break;
			} 
		}

		if(result == null)
		{
			GameObject[] oldPool = _pool;
			_pool = new GameObject[_size + 1];
			for(int i = 0;i < _size;++i)
			{
				_pool[i] = oldPool[i];
			}
			_pool[_size] = GameObject.Instantiate<GameObject>(_prefab);
			_pool[_size].transform.parent = _parent;
			_pool[_size].gameObject.SetActive(false);
			result = _pool[_size];
			++_size;
        }

		return result;
	}

	#endregion Methods
}
