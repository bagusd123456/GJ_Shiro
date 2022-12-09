using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagement : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;
	public static SoundManagement Instance {get;set;}
	public AudioSource music;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			//DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SetVolume (float sliderVolume)
	{
		audioMixer.SetFloat("volume",Mathf.Log10(sliderVolume)*20);
	}
}
