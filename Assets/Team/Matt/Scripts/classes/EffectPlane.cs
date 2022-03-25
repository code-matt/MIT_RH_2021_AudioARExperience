using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlane : MonoBehaviour
{
	public GameObject plane;
	public int id;

	private float sizeX;
	private float sizeY;

    void Start()
    {
        StartCoroutine(UpdatePlaneSize());
    }

    public void updateSize(float sizeX, float sizeY)
    {
		this.sizeX = sizeX;
		this.sizeY = sizeY;
    }

    IEnumerator UpdatePlaneSize()
    {
       Debug.Log("Updating plane size");
       if(this.plane != null)
       {
            plane.transform.localScale.Set(this.sizeX, this.sizeY, 1.0f);
       }
       yield return new WaitForSeconds(.5f);
    }

    void Update()
    {
        
    }


}
