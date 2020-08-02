using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Sound")]
public class Sound : ScriptableObject
{
	public Clip[] clips;
	public bool loop;

	public float fadeInTime = 0f;
	public float fadeOutTime = 0f;

	[HideInInspector]
	public AudioSource source;

	// Return a random clip from the list
	public Clip GetRandomClip() {
		if (clips.Length == 0) {
			//Debug.LogError("Cannot GetRandomClip for Sound " + this + ": no clips found.");
			return null;
		}

		int randIndex = Random.Range(0, clips.Length);
		return clips[randIndex];
	}

	public Clip GetClip(int index) {
		if (index < 0 || index > clips.Length -1)
		{
			Debug.LogError("The index " + index + " for clip " + name + " does not exist.");
			return null;
		}
		return clips[index];
	}

}
