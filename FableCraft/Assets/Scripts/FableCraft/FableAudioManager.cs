using UnityEngine;

public class FableAudioManager : MonoBehaviour, IFableAudioManager
{
	// Audio players components.
	public AudioSource _effectsSource;
	public AudioSource _musicSource;
	public AudioSource _textSource;

	// Singleton instance.
	public static FableAudioManager Instance = null;

	// Initialize the singleton instance.
	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
		//DontDestroyOnLoad(gameObject);
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
	public void PlaySFX(AudioClip clip, float volume)
	{
		_effectsSource.clip = clip;
		_effectsSource.volume = volume;
		_effectsSource.Play();
	}

	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip, float volume)
	{
		_musicSource.clip = clip;
		_musicSource.volume = volume;
		_musicSource.Play();
	}

	// Play a single clip through the music source.
	public void PlayTextEffectAudio(AudioClip clip, float volume)
	{
		_textSource.clip = clip;
		_textSource.volume = volume;
		_textSource.Play();
	}

	// Play a random clip from an array, and randomize the pitch slightly.
	public void PlayRandomSFX(float volume, float lowPitchRange, float highPitchRange, params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		_effectsSource.pitch = randomPitch;
		_effectsSource.clip = clips[randomIndex];
		_effectsSource.volume = volume;
		_effectsSource.Play();
	}
}
