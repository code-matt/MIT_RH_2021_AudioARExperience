using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncRippleShader : AudioSyncer
{

	public float audioLevel;
	public float frequencyLevel;

	public Material reactiveMat;
	public string audioLevelPropertyName_amplitude;
	public string audioLevelPropertyName_frequency;

	public float levelMax = 1.0f;
	public float freqMax = 1.0f;
	public float levelMin = 0f;

	private IEnumerator UpdateAmpProperties(float _ampTarget)
	{
		
		float _currA = audioLevel;
		float _initialA = _currA;
		float _timerA = 0;

		while (_currA != _ampTarget)
		{
			_currA = Mathf.Lerp(_initialA, _ampTarget, _timerA / timeToBeat);
			_timerA += Time.deltaTime;

			audioLevel = _currA;

			yield return null;
		}

		m_isBeat = false;
	}

	private IEnumerator UpdateFreqProperties(float _freqTarget)
	{
		float _currF = frequencyLevel;
		float _initialF = _currF;
		float _timerF = 0;

		while (_currF != _freqTarget)
		{
			_currF = Mathf.Lerp(_initialF, _freqTarget, _timerF / timeToBeat);
			_timerF += Time.deltaTime;
		
			frequencyLevel = _currF;
		
			yield return null;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (m_isBeat) return;

		audioLevel = Mathf.Lerp(audioLevel, levelMin, restSmoothTime * Time.deltaTime);
		reactiveMat.SetFloat(audioLevelPropertyName_amplitude, audioLevel);

		frequencyLevel = Mathf.Lerp(frequencyLevel, levelMin, restSmoothTime * Time.deltaTime);
		reactiveMat.SetFloat(audioLevelPropertyName_frequency, frequencyLevel);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		StopCoroutine("UpdateAmpProperties");
		StopCoroutine("UpdateFreqProperties");

		StartCoroutine("UpdateAmpProperties", levelMax);
		StartCoroutine("UpdateFreqProperties", freqMax);
	}
}
