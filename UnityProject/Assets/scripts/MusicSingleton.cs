using UnityEngine;
using System.Collections;

public class MusicSingleton : MonoBehaviour {
	
	private static MusicSingleton instance = null;
	
	public static MusicSingleton Instance {
		get { return instance; }
	}
	
	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			audio.Stop();
			if(instance.audio.clip != audio.clip)
			{
				instance.audio.clip = audio.clip;
				instance.audio.volume = audio.volume;
				instance.audio.Play();
			}
			
			Destroy(this.gameObject);
			return;
		} 
		instance = this;
		audio.Play ();
		DontDestroyOnLoad(this.gameObject);
	}
}