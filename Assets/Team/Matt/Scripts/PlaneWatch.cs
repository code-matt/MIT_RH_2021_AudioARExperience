using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWatch : MonoBehaviour
{
    public GameObject mockPlanes;
    public GameObject effectPlanes;

    private List<MockPlane> actualMockPlanes = null;
    private List<EffectPlane> actualEffectPlanes = null;

    void Start()
    {
        EffectPlane effectPlanesParent = gameObject.GetComponent<EffectPlane>();
        MockPlanes mockPlanesParent = gameObject.GetComponent<MockPlanes>();
        List<MockPlane> actualMockPlanes = mockPlanesParent.planesArray;
        // List<EffectPlane> effectPlanes = effectPlanesParent.;
    }

    void Update()
    {
        if (actualMockPlanes != null)
        {
            foreach (var x in actualMockPlanes)
            {
                Debug.Log(x.id);
            }
        }
    }
}
