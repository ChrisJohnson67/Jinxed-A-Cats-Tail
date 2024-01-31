using FMOD.Studio;
using FMODUnity;
using Platform.Utility;

public class SoundManager : MonoSingleton<SoundManager>
{

	private EventInstance m_currentMusic;

	public void PlaySound(EventReference a_sound)
	{
		if (!a_sound.IsNull)
			RuntimeManager.PlayOneShot(a_sound);
	}

	public void PlayMusic(EventReference a_music)
	{
		if (m_currentMusic.isValid())
		{
			StopCurrentMusic(true);
		}
		m_currentMusic = RuntimeManager.CreateInstance(a_music);
		m_currentMusic.start();
	}

	public void StopCurrentMusic(bool a_fade = true)
	{
		if (m_currentMusic.isValid())
		{
			m_currentMusic.release();
			m_currentMusic.stop(a_fade ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
			m_currentMusic.clearHandle();
		}
	}
}
