using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private Transform _transform = null;

	private static Audio _instance = null;
	public static Audio Instance { get { return _instance; } }

	private int _nextSourceIndexToUse = 0;

	private AudioSourceObjectPool _audioSourceObjectPool = null;

	private Transform _playerCameraTransform = null;

	private AudioSource _musicSource = null;

	[SerializeField]
	private AudioClip _music = null;

	#endregion Variables

	#region Monobehaviour Methods

	void Awake()
	{
		Initialize();
	}

	void Update()
	{

	}

	#endregion Monobehaviour Methods

	#region Methods

	private void Initialize()
	{
		Audio._instance = this;

        UnityEngine.SceneManagement.Scene startScene = new UnityEngine.SceneManagement.Scene();
		int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
		for(int i = 0;i < sceneCount;++i)
		{
			UnityEngine.SceneManagement.Scene tmpScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

            if (tmpScene.name == GameLord.startSceneName)
			{
				startScene = tmpScene;
				break;
			}
		}

		

		_audioSourceObjectPool = new AudioSourceObjectPool(_transform, 24);
		int sourceCount = _audioSourceObjectPool.Size;
		AudioSource[] sources = _audioSourceObjectPool.Pool;
		//for(int i = 0;i < sourceCount;++i)
		//{
		//	UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(sources[i].gameObject, startScene);
		//}

		if (_music)
		{
			_musicSource = this.gameObject.AddComponent<AudioSource>();
			_musicSource.loop = true;
			_musicSource.Play();
			_musicSource.volume = 0.0f;
		}
	}

	private void DisableAllSound()
	{
		_audioSourceObjectPool.StopAllSources();
	}

	public AudioSource PlaySound(AudioClip[] clips, float volume = 1.0f)
	{
		AudioSource usedSource = null;

		int clipCount = 0;

		if (clips != null && (clipCount = clips.Length) > 0)
		{
			int clipToPlay = UnityEngine.Random.Range(0, clipCount);

			usedSource = _audioSourceObjectPool.GetPooledObject();

			usedSource.transform.parent = _transform;
			usedSource.transform.localPosition = Vector3.zero;

			usedSource.clip = clips[clipToPlay];
			usedSource.volume = volume;
			usedSource.Play();
		}

		return usedSource;
	}

	public AudioSource PlaySound(AudioClip[] clips, Vector3 position ,float volume = 1.0f)
	{
		AudioSource usedSource = null;

		int clipCount = 0;

		if(clips != null && (clipCount = clips.Length) > 0)
		{
			int clipToPlay = UnityEngine.Random.Range(0, clipCount);

			usedSource = _audioSourceObjectPool.GetPooledObject();

			usedSource.transform.parent = null;
			usedSource.transform.position = position;

			usedSource.clip = clips[clipToPlay];
			usedSource.volume = volume;
			usedSource.Play();
		}

		return usedSource;
	}

	public void SetMusicVolume(float volume)
	{
		if(_musicSource != null)
		{
			_musicSource.volume = volume;
		}
	}

	#endregion Methods
}
