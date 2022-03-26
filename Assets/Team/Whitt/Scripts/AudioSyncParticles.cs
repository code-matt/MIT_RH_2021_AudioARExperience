using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncParticles : AudioSyncer
{
	public Vector3 planeScale;
	
	public ParticleSystem psReactive;
	public Material psMat;

	public float psSpeed;

	public float audioLevel;

	public float levelMax = 1.0f;
	public float levelMin = 0f;

	public Color[] beatColors;
	public Color restColor;

	private int m_randomIndx;
	private Color m_col;

	private IEnumerator BurstEmitter(float _burstTarget)
	{
		var em = psReactive.emission;

		float _currA = audioLevel;
		float _initialA = _currA;
		float _timerA = 0;

		while (_currA != _burstTarget)
		{
			_currA = Mathf.Lerp(_initialA, _burstTarget, _timerA / timeToBeat);
			_timerA += Time.deltaTime;

			audioLevel = _currA;

			em.SetBurst(0, new ParticleSystem.Burst(2.0f, 100));

			yield return null;
		}

		m_isBeat = false;
	}

	private IEnumerator UpdateSpeed(float _speedTarget)
	{
		float _currF = psSpeed;
		float _initialF = _currF;
		float _timerF = 0;

		while (_currF != _speedTarget)
		{
			_currF = Mathf.Lerp(_initialF, _speedTarget, _timerF / timeToBeat);
			_timerF += Time.deltaTime;

			psSpeed = _currF;

			yield return null;
		}
	}

	private IEnumerator MoveToColor(Color _target)
	{
		Color _curr = m_col;
		Color _initial = _curr;
		float _timer = 0;

		while (_curr != _target)
		{
			_curr = Color.Lerp(_initial, _target, _timer / timeToBeat);
			_timer += Time.deltaTime;

			m_col = _curr;

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

		//update particle system properties
		psSpeed = Mathf.Lerp(psSpeed, levelMin, restSmoothTime * Time.deltaTime);
		
		var psVal = psReactive.velocityOverLifetime;
		psVal.zMultiplier = psSpeed * 10f;

		var emVal = psReactive.emission;

		//Debug.Log(planeScale);

		if (planeScale.x * planeScale.y < 1)
        {
			emVal.rateOverTime = planeScale.x * planeScale.y * 2.0f;
        }
		if(planeScale.x * planeScale.y >= 1 && planeScale.x * planeScale.y < 5)
        {
			emVal.rateOverTime = planeScale.x * planeScale.y * 5.0f;
        }
		if(planeScale.x * planeScale.y >= 5)
        {
			emVal.rateOverTime = planeScale.x * planeScale.y * 10.0f;
        }
	}

	public override void OnBeat()
	{
		base.OnBeat();

		// Refresh Coroutines to modify properties

		StopCoroutine("UpdateSpeed");
		StopCoroutine("BurstEmitter");

		StartCoroutine("UpdateSpeed", levelMax);
		StartCoroutine("BurstEmitter", levelMax);

		Color _c = RandomColor();

		StopCoroutine("MoveToColor");
		StartCoroutine("MoveToColor", _c);
	}

	private void Start()
	{
		psReactive = GetComponent<ParticleSystem>();

		bool hasMat = psReactive.GetComponent<Material>();

		if (hasMat != false)
		{
			m_col = GetComponent<Material>().color;
		}
	}
}
