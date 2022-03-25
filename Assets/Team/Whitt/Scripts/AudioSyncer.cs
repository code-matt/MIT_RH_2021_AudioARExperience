using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour {

	/// <summary>
	/// Code from Renaissance Coders Unity Tutorial on creating an audio visualizer
	/// </summary>


	public float bias; //wave spectrum value to track
	public float timeStep; //minimum interval to look for beats
	public float timeToBeat; //how long it takes to change the target values
	public float restSmoothTime; //how long it takes to reset target values

	private float m_previousAudioValue;
	private float m_audioValue;
	private float m_timer;

	protected bool m_isBeat;

	public virtual void OnBeat()
	{
		Debug.Log("beat");
		m_timer = 0;
		m_isBeat = true;
	}

	/// <summary>
	/// Inherit this to do whatever you want in Unity's update function
	/// Typically, this is used to arrive at some rest state..
	/// ..defined by the child class
	/// </summary>
	public virtual void OnUpdate()
	{ 
		// update audio value
		m_previousAudioValue = m_audioValue;
		m_audioValue = AudioSpectrum.spectrumValue;

		// if audio value went below the bias during this frame
		if (m_previousAudioValue > bias &&
			m_audioValue <= bias)
		{
			// if minimum beat interval is reached
			if (m_timer > timeStep)
				OnBeat();
		}

		// if audio value went above the bias during this frame
		if (m_previousAudioValue <= bias &&
			m_audioValue > bias)
		{
			// if minimum beat interval is reached
			if (m_timer > timeStep)
				OnBeat();
		}

		m_timer += Time.deltaTime;
	}

	private void Update()
	{
		OnUpdate();
	}
}