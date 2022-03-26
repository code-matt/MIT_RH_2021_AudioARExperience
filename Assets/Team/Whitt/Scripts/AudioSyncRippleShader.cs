using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncRippleShader : AudioSyncer
{

	public float audioLevel;
	public float frequencyLevel;

	public Material reactiveMat;

	public string PropertyName_amplitude;
	public string PropertyName_frequency;
	public string PropertyName_color;


	public float levelMax = 1.0f;
	public float freqMax = 1.0f;
	public float levelMin = 0f;

	public Color[] beatColors;
	public Color restColor;

	private int m_randomIndx;
	private Color m_img;

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

	private IEnumerator MoveToColor(Color _target)
	{
		Color _curr = m_img;
		Color _initial = _curr;
		float _timer = 0;

		while (_curr != _target)
		{
			_curr = Color.Lerp(_initial, _target, _timer / timeToBeat);
			_timer += Time.deltaTime;

			m_img = _curr;

			yield return null;
		}

		m_isBeat = false;
	}

	private Color RandomColor()
	{
		if (beatColors == null || beatColors.Length == 0) return Color.white;
		m_randomIndx = Random.Range(0, beatColors.Length);
		return beatColors[m_randomIndx];
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (m_isBeat) return;

		audioLevel = Mathf.Lerp(audioLevel, levelMin, restSmoothTime * Time.deltaTime);
		reactiveMat.SetFloat(PropertyName_amplitude, audioLevel);

		frequencyLevel = Mathf.Lerp(frequencyLevel, levelMin, restSmoothTime * Time.deltaTime);
		reactiveMat.SetFloat(PropertyName_frequency, frequencyLevel);

		m_img = Color.Lerp(m_img, restColor, restSmoothTime * Time.deltaTime);
		reactiveMat.SetColor(PropertyName_color, m_img);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		StopCoroutine("UpdateAmpProperties");
		StopCoroutine("UpdateFreqProperties");

		StartCoroutine("UpdateAmpProperties", levelMax);
		StartCoroutine("UpdateFreqProperties", freqMax);

		Color _c = RandomColor();

		StopCoroutine("MoveToColor");
		StartCoroutine("MoveToColor", _c);
	}

	private void Start()
	{
		bool hasMat = GetComponent<Material>();
		
		if(hasMat != false)
        {
			m_img = GetComponent<Material>().color;
		}
	}
}
