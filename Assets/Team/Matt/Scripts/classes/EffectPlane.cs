using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlane : MonoBehaviour
{
	public GameObject plane;
	public int id;

	private float sizeX = 0.1f;
	private float sizeY = 0.1f;
    private float posX;
    private float posY;
    public bool stillUsed = false;

    void Start()
    {
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        plane.transform.Rotate(Vector3.up * 90);
        StartCoroutine("UpdatePlaneSize");
    }

    public void updateSize(float _sizeX, float _sizeY)
    {
        //Debug.Log("Updating size to: " + _sizeX);
		sizeX = _sizeX;
		sizeY = _sizeY;
    }

    public void updatePosition(float _posX, float _posY)
    {
        //Debug.Log("Updating size to: " + _sizeX);
        posX = _posX;
        posY = _posY;
        if(plane)
        {
            Debug.Log("Updating Position For.." + id);
            plane.transform.position.Set(_posX, posY, 0.0f);
;       }
    }

    IEnumerator UpdatePlaneSize()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (plane)
            {
                //Debug.Log("Updating plane size " + id);
                plane.transform.localScale = new Vector3(sizeX, 0.1f, sizeY);
            }
        }
    }

    void Update()
    {
        
    }


}
