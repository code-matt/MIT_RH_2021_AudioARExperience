using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlane : MonoBehaviour
{
	public GameObject plane;
	public int id;

	private float sizeX;
	private float sizeY;

    IEnumerator Start()
    {
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.Rotate(Vector3.up * 90);
        yield return StartCoroutine("UpdatePlaneSize");
    }

    public void updateSize(float _sizeX, float _sizeY)
    {
        Debug.Log("Updating size to: " + _sizeX);
		sizeX = _sizeX;
		sizeY = _sizeY;
    }

    IEnumerator UpdatePlaneSize()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (plane)
            {
                Debug.Log("Updating plane size " + id);
                plane.transform.localScale = new Vector3(sizeX, 0.1f, sizeY);
            }
        }
    }

    void Update()
    {
        
    }


}
