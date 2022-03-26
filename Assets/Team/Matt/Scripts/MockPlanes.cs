using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockPlanes : MonoBehaviour
{
    // Start is called before the first frame update
    public List<MockPlane> mockPlanes = new List<MockPlane>(); 

    void Start()
    {
        for (int i = 0; i < 1; i++)
        {
           PlaneWatch planeContainer = gameObject.GetComponentInParent<PlaneWatch>();
           MockPlane mockPlane = planeContainer._mockPlanesContainer.AddComponent<MockPlane>();
           mockPlanes.Add(mockPlane);
           //mockPlane.plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
           //mockPlane.plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
           //mockPlane.transform.Rotate(Vector3.up * 90);
           mockPlane.id = Random.Range(0, 47465);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
