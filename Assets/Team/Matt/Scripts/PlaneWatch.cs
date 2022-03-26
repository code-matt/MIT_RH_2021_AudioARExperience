using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class PlaneWatch : MonoBehaviour
    {

        public GameObject PlanePrefabGO;
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
                foreach (var x in _mockPlanesScript.mockPlanes)
                {
                    EffectPlane foundPlane = _effectPlanesScript.effectPlanes.Find(f => f.id == x.id);
                    if (foundPlane) {
                        // this is where we will pass the x/y size from HL2 plane to ours
                        foundPlane.updateSize(
                            Random.Range(0.1f, 0.2f),
                            Random.Range(0.1f, 0.2f)
                        );
                    } else {
                    var planeW = Instantiate(PlanePrefabGO);//gameObject.GetComponentInParent<PlaneWatch>();

                        EffectPlane newEffectPlane = _effectPlanesContainer.AddComponent<EffectPlane>();
                        newEffectPlane.id = x.id;
                        // newEffectPlane.plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        _effectPlanesScript.effectPlanes.Add(newEffectPlane);
                        Debug.Log("Added new effect plane");
                    }
                }
            } else
            {
                Debug.Log("mock planes script is null");
            }
        }
    }
