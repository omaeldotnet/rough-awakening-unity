using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
	public PlayableDirector director;

	private void Start()
	{
		Debug.Log("Starting Timeline...");
		director.Play();
	}

	public void PlayTimeline()
	{
		director.Play();
	}

	public void SkipTimelineTo(float time)
	{
		director.time = time;
		director.Play();
	}
}