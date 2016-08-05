using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
	#region Variables

	private static Audio _instance = null;
	public static Audio Instance { get { return _instance; } }

	private AudioSource[] _sources = null;
	private int _sourceCount = 24;

	private int _nextSourceIndexToUse = 0;

	#endregion Variables

	#region Monobehaviour Methods

	void Awake()
	{
		Initialize();
	}

	#endregion Monobehaviour Methods

	#region Methods

	private void Initialize()
	{
		Audio._instance = this;

		_sources = new AudioSource[_sourceCount];
		for(int i = 0;i < _sourceCount;++i)
		{
			_sources[i] = this.gameObject.AddComponent<AudioSource>();
		}
	}

	private void DisableAllSound()
	{
		for(int i = 0;i < _sourceCount;++i)
		{
			_sources[i].Stop();
		}
		_nextSourceIndexToUse = 0;
	}

	public AudioSource PlaySound(AudioClip[] clips,float volume = 1.0f)
	{
		AudioSource usedSource = null;

		int clipCount = 0;

		if(clips != null && (clipCount = clips.Length) > 0)
		{
			int clipToPlay = UnityEngine.Random.Range(0, clipCount);

			usedSource = _sources[_nextSourceIndexToUse];

			_nextSourceIndexToUse = (_nextSourceIndexToUse + 1) % _sourceCount;

			usedSource.clip = clips[clipToPlay];
			usedSource.volume = volume;
			usedSource.Play();
		}

		return usedSource;
	}

	#endregion Methods
}
