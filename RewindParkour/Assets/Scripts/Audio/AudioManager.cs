using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;

	[SerializeField]
	private AudioMixerGroup audioMixerGroup = default;
	
	private void Start()
    {	
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.loop = s.loop;
			s.source.outputAudioMixerGroup = audioMixerGroup;

		}
    }

	public void Play (string name) 
	{
		StartPlay(GetSound(name));
	}

	public void Stop (string name)
	{
		StopPlay(GetSound(name));
	}

	public void StopImmediate(string name)
	{
		StopPlay(GetSound(name), true);
	}

	public void Toggle (string name) 
	{
		Sound s = GetSound(name);

		if (s?.source.isPlaying ?? true) 
		{
			StopPlay(s);
		}
		else
		{
			StartPlay(s);
		}
	}

	public void PlayOneShotAtLocation(string name, GameObject location, float volumeMultiplier = 1, float pitchMultiplier = 1)
	{
		Sound s = GetSound(name);

		if (s == null || location == null)
		{
			Debug.LogError("Cannot PlayOneShotAtLocation: no non-empty sound named " + name + " or gameobject");
			return;
		}

		Clip clip = s.GetRandomClip();
		if (clip == null) return; 

		s.source = location.AddComponent<AudioSource>();
		s.source.spatialBlend = .7f;
		s.source.volume = clip.volume * volumeMultiplier;
		s.source.pitch = clip.pitch * pitchMultiplier;

		s.source.PlayOneShot(clip.AudioClip);
		StartCoroutine(removeAudioSource(s.source, clip.AudioClip.length));
	}

	IEnumerator removeAudioSource(AudioSource audioSource, float time)
	{
		yield return new WaitForSeconds(time);
		Destroy(audioSource);
	}
	


	public void PlayOneShot(string name, float volumeMultiplier = 1, float pitchMultiplier = 1) 
	{
		Sound s = GetSound(name);

		if (s != null && s.source != null) 
		{
			Clip clip = s.GetRandomClip();
			if (clip == null) return; 

			s.source.volume = clip.volume * volumeMultiplier;
			s.source.pitch = clip.pitch * pitchMultiplier;

			s.source.PlayOneShot(clip.AudioClip);
		}
		else 
		{
			Debug.LogError("Cannot PlayOneShot: no non-empty sound named " + name);
		}
	}


	private Sound GetSound(string name) {
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
		{
			Debug.LogError("Sound: " + name + " not found");
		}

		return s;
	}

	public bool HasSound(string name) {
		return Array.Exists(sounds, sound => sound.name == name);
	}


	private void StartPlay(Sound s)
	{
		if (s == null || s.clips.Length == 0 || s.source == null || s.source.isPlaying) return;
		
		Clip clip = s.GetRandomClip();
		s.source.clip = clip.AudioClip;
		s.source.volume = clip.volume;
		s.source.pitch = clip.pitch;

		if (s.fadeInTime > 0) {
			if (s.source.DOKill() == 0)
			{
				// initialize volume when not already fading
				s.source.volume = 0;
			}

			s.source.DOFade(clip.volume, s.fadeInTime);
		}

		s.source.Play();
	}

	private void StopPlay(Sound s, bool ignoreFade)
	{
		if (s == null || s.clips.Length == 0 || s.source == null) return;

		if (s.fadeOutTime > 0 && !ignoreFade)
		{
			s.source.DOKill();
			s.source.DOFade(0, s.fadeOutTime);
		}
		else
		{
			s.source.Stop();
		}
	}
	private void StopPlay(Sound s)
	{
		if (s == null || s.source == null) return;

		if (s.fadeOutTime > 0)
		{
			s.source.DOKill();
			s.source.DOFade(0, s.fadeOutTime);
		}
		else
		{
			s.source.Stop();
		}
	}
}
