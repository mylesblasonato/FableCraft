using UnityEngine;

namespace FableCraft
{
	public class FableAudioManager : MonoBehaviour, IFableAudioManager
	{
		// Audio players components.
		[SerializeField] AudioSource _effectsSource;
		[SerializeField] AudioSource _voiceSource;
		[SerializeField] AudioSource _musicSource;
		[SerializeField] AudioSource _textSource;
		[SerializeField] AudioClip[] _sfxClips, _musicClips;

		// Singleton instance.
		public static FableAudioManager Instance {get; private set;}

		// Initialize the singleton instance.
		void Awake()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
		}

		// Stop all audio
		public void StopAudio()
		{
			_effectsSource.Stop();
			_musicSource.Stop();
		}

		public void StopTextEffectAudio()
		{
			_textSource.Stop();
		}

		// Play a single clip through the sound effects source.
		public void PlaySfx(int clip, float volume)
		{
			if (_sfxClips.Length == 0) return;
			_effectsSource.clip = _sfxClips[clip];
			_effectsSource.volume = volume;
			_effectsSource.Play();
		}

		// Play a single clip through the voice effects source.
		public void PlayVo(AudioClip clip, float volume)
		{
			if (clip == null) return;
			_voiceSource.clip = clip;
			_voiceSource.volume = volume;
			_voiceSource.Play();
		}

		// Play a single clip through the music source.
		public void PlayMusic(int clip, float volume)
		{
			if (_musicClips.Length == 0) return;
			_musicSource.clip = _musicClips[clip];
			_musicSource.volume = volume;
			_musicSource.Play();
		}

		// Play a single clip through the music source.
		public void PlayTextEffectAudio(AudioClip clip, float volume)
		{
			if (clip == null) return;
			_textSource.clip = clip;
			_textSource.volume = volume;
			_textSource.Play();
		}
	}
}
