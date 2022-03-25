using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWatch : MonoBehaviour
{

    public GameObject mockPlanesGO;

    public MockPlanes _mockPlanesScript = null;
    //private List<EffectPlane> actualEffectPlanes = null;

    void Start()
    {
        //EffectPlane effectPlanesParent = gameObject.GetComponent<EffectPlane>();
        _mockPlanesScript = mockPlanesGO.GetComponent<MockPlanes>();
        // List<EffectPlane> effectPlanes = effectPlanesParent.;
    }

    void Update()
    {
        if (_mockPlanesScript != null)
        {
            foreach (var x in _mockPlanesScript.planes)
            {
                Debug.Log(x.id);
            }
        } else
        {
           Debug.Log("mock planes script is null");
        }
    }
}
