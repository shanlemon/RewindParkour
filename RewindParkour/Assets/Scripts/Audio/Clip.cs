using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Clip 
{
	public AudioClip AudioClip;
	[Range(0f, 2f)]
	public float volume = 1;
	[Range(.1f, 3f)]
	public float pitch = 1;

}
