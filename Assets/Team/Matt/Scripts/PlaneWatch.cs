using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWatch : MonoBehaviour
{

    public GameObject mockPlanesGO;
    public GameObject effectPlanesGO;

    public MockPlanes _mockPlanesScript = null;
    public EffectPlanes _effectPlanesScript = null;
    //private List<EffectPlane> actualEffectPlanes = null;

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
                Debug.Log(x.id);
                if (_effectPlanesScript.planes.Find(f => f.id == x.id)) {
                    Debug.Log("Existing effect Plane Found");
                } else
                {
                    EffectPlane plane = ScriptableObject.CreateInstance<EffectPlane>();
                    plane.id = x.id;
                    _effectPlanesScript.planes.Add(plane);
                    Debug.Log("Added new effect plane");
                }
            }
        } else
        {
           Debug.Log("mock planes script is null");
        }
    }
}
