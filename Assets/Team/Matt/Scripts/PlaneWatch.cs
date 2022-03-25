using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWatch : MonoBehaviour
{

    public GameObject mockPlanesGO;
    public GameObject effectPlanesGO;

    public MockPlanes _mockPlanesScript = null;
    public EffectPlanes _effectPlanesScript = null;

    public GameObject _mockPlanesContainer = null;
    public GameObject _effectPlanesContainer = null;

    void Start()
    {
        _mockPlanesScript = mockPlanesGO.GetComponent<MockPlanes>();
        _effectPlanesScript = effectPlanesGO.GetComponent<EffectPlanes>();
    }

    void Update()
    {
        if (_mockPlanesScript != null)
        {
            foreach (var x in _mockPlanesScript.planes)
            {
                EffectPlane foundPlane = _effectPlanesScript.planes.Find(f => f.id == x.id);
                if (foundPlane) {
                    // this is where we will pass the x/y size from HL2 plane to ours
                    foundPlane.updateSize(
                        Random.Range(0f, 2f),
                        Random.Range(0f, 2f)
                    );
                } else
                {
                    PlaneWatch planeW = gameObject.GetComponentInParent<PlaneWatch>();
                    EffectPlane newPlane = planeW._effectPlanesContainer.AddComponent<EffectPlane>();
                    newPlane.id = x.id;
                    _effectPlanesScript.planes.Add(newPlane);
                    Debug.Log("Added new effect plane");
                }
            }
        } else
        {
           Debug.Log("mock planes script is null");
        }
    }
}
