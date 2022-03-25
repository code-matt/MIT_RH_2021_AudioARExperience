using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncShader : AudioSyncer
{

	public float audioLevel;

	public Material reactiveMat;
	public string audioLevelPropertyName;

	public float levelMax = 1.0f;
	public float levelMin = 0f;

    private IEnumerator UpdateAudioLevel(float _target)
	{
		float _curr = audioLevel;
		float _initial = _curr;
		float _timer = 0;

		while (_curr != _target)
		{
			_curr = Mathf.Lerp(_initial, _target, _timer / timeToBeat);
			_timer += Time.deltaTime;

			audioLevel = _curr;

			yield return null;
		}

		m_isBeat = false;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (m_isBeat) return;

		audioLevel = Mathf.Lerp(audioLevel, levelMin, restSmoothTime * Time.deltaTime);
		reactiveMat.SetFloat(audioLevelPropertyName, audioLevel);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		StopCoroutine("UpdateAudioLevel");
		StartCoroutine("UpdateAudioLevel", levelMax);
	}
}
