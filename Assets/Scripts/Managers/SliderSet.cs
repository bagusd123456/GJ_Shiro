using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SliderSet : MonoBehaviour
{
	public Slider sliderVolume;


	public void SliderVolume()
	{
		SoundManagement.Instance.music.volume = sliderVolume.value;
	}
}
