using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockPlanes : MonoBehaviour
{
    // Start is called before the first frame update
    public List<MockPlane> planes = new List<MockPlane>(); 

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
           PlaneWatch planeContainer = gameObject.GetComponentInParent<PlaneWatch>();
           MockPlane plane = planeContainer._mockPlanesContainer.AddComponent<MockPlane>();
           planes.Add(plane);
           plane.plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
           plane.id = Random.Range(0, 47465);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
