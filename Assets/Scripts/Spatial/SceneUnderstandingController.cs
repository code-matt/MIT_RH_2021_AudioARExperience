using Microsoft.MixedReality.Toolkit.Experimental.SpatialAwareness;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using UnityEngine;

namespace ASV
{
    public class SceneUnderstandingController : SpatialMeshHandler, IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessSceneObject>
    {
        #region Private Fields

        #region Serialized Fields

        [SerializeField]
        private string SavedSceneNamePrefix = "SceneUnderstanding";
        [SerializeField]
        private bool InstantiatePrefabs = false;
        [SerializeField]
        private GameObject InstantiatedPrefab = null;
        [SerializeField]
        private Transform InstantiatedParent = null;

        #endregion Serialized Fields

        private IMixedRealitySceneUnderstandingObserver observer;

        private List<GameObject> instantiatedPrefabs;

        private Dictionary<SpatialAwarenessSurfaceTypes, Dictionary<int, SpatialAwarenessSceneObject>> observedSceneObjects;

        #endregion Private Fields

        #region MonoBehaviour Functions

        /// <summary>
        /// REALITY HACK CODE
        public GameObject mockPlanesGO;
        public GameObject effectPlanesGO;

        public MockPlanes _mockPlanesScript = null;
        public EffectPlanes _effectPlanesScript = null;

        public GameObject _mockPlanesContainer = null;
        public GameObject _effectPlanesContainer = null;
        /// </summary>



        protected override void Start()
        {
            observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySceneUnderstandingObserver>();

            if (observer == null)
            {
                Debug.LogError("Couldn't access Scene Understanding Observer! Please make sure the current build target is set to Universal Windows Platform. "
                    + "Visit https://docs.microsoft.com/windows/mixed-reality/mrtk-unity/features/spatial-awareness/scene-understanding for more information.");
                return;
            }
            instantiatedPrefabs = new List<GameObject>();
            observedSceneObjects = new Dictionary<SpatialAwarenessSurfaceTypes, Dictionary<int, SpatialAwarenessSceneObject>>();

            ////
            ///REALITY HACK CODE
            ///
            _mockPlanesScript = mockPlanesGO.GetComponent<MockPlanes>();
            _effectPlanesScript = effectPlanesGO.GetComponent<EffectPlanes>();
        }

        protected override void OnEnable()
        {
                Debug.Log($"IMixedRealitySpatialAwarenessObservationHandler>OnEnable");
            RegisterEventHandlers<IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessSceneObject>, SpatialAwarenessSceneObject>();
        }

        protected override void OnDisable()
        {
            UnregisterEventHandlers<IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessSceneObject>, SpatialAwarenessSceneObject>();
        }

        protected override void OnDestroy()
        {
            UnregisterEventHandlers<IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessSceneObject>, SpatialAwarenessSceneObject>();
        }

        #endregion MonoBehaviour Functions

        #region IMixedRealitySpatialAwarenessObservationHandler Implementations

        public void OnObservationAdded(MixedRealitySpatialAwarenessEventData<SpatialAwarenessSceneObject> eventData)
        {
            Debug.Log("Hello");
            Debug.Log($"OnObservationAdded");
            // This method called everytime a SceneObject created by the SU observer
            // The eventData contains everything you need do something useful

            AddToData(eventData.Id);

            if (observedSceneObjects.TryGetValue(eventData.SpatialObject.SurfaceType, out Dictionary<int, SpatialAwarenessSceneObject> sceneObjectDict))
            {
                sceneObjectDict.Add(eventData.Id, eventData.SpatialObject);
            }
            else
            {
                observedSceneObjects.Add(eventData.SpatialObject.SurfaceType, new Dictionary<int, SpatialAwarenessSceneObject> { { eventData.Id, eventData.SpatialObject } });
            }
            
            if (InstantiatePrefabs && eventData.SpatialObject.Quads.Count > 0)
            {
                var prefab = Instantiate(InstantiatedPrefab);
                prefab.transform.SetPositionAndRotation(eventData.SpatialObject.Position, eventData.SpatialObject.Rotation);
                float sx = eventData.SpatialObject.Quads[0].Extents.x;
                float sy = eventData.SpatialObject.Quads[0].Extents.y;
                Debug.Log($"InstantiatePrefab {sx},{sy}");
                prefab.transform.localScale = new Vector3(sx, sy, .1f);
                if (InstantiatedParent)
                {
                    prefab.transform.SetParent(InstantiatedParent);
                }
                instantiatedPrefabs.Add(prefab);
            }
            else
            {
                foreach (var quad in eventData.SpatialObject.Quads)
                {
                    Debug.Log(quad);
                    EffectPlane foundPlane = _effectPlanesScript.effectPlanes.Find(f => f.id == quad.Id);
                    if (foundPlane)
                    {
                        // this is where we will pass the x/y size from HL2 plane to ours
                        //foundPlane.updateSize(
                            //Random.Range(0.1f, 0.2f),
                            //Random.Range(0.1f, 0.2f)
                        //);
                        foundPlane.updatePosition(
                            quad.Extents.x,
                            quad.Extents.y

                        );
                        foundPlane.stillUsed = true;
                    }
                    else
                    {
                        //PlaneWatch planeW = gameObject.GetComponentInParent<PlaneWatch>();
                        EffectPlane newEffectPlane = _effectPlanesContainer.AddComponent<EffectPlane>();
                        newEffectPlane.id = quad.Id;
                        // newEffectPlane.plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        _effectPlanesScript.effectPlanes.Add(newEffectPlane);
                        Debug.Log("Added new effect plane");
                    }
                    //quad.GameObject.GetComponent<Renderer>().material.color = ColorForSurfaceType(eventData.SpatialObject.SurfaceType);
                }
                foreach (var effectP in _effectPlanesScript.effectPlanes)
                {
                    if (!effectP.stillUsed)
                    {
                        Debug.Log("Destroying effectPlane for: " + effectP.id);
                        Destroy(effectP.plane);
                        Destroy(effectP);
                    }
                    effectP.stillUsed = false;
                }

            }
        }

        public void OnObservationUpdated(MixedRealitySpatialAwarenessEventData<SpatialAwarenessSceneObject> eventData)
        {
            UpdateData(eventData.Id);
            
            if (observedSceneObjects.TryGetValue(eventData.SpatialObject.SurfaceType, out Dictionary<int, SpatialAwarenessSceneObject> sceneObjectDict))
            {
                observedSceneObjects[eventData.SpatialObject.SurfaceType][eventData.Id] = eventData.SpatialObject;
            }
            else
            {
                observedSceneObjects.Add(eventData.SpatialObject.SurfaceType, new Dictionary<int, SpatialAwarenessSceneObject> { { eventData.Id, eventData.SpatialObject } });
            }
        }

        public void OnObservationRemoved(MixedRealitySpatialAwarenessEventData<SpatialAwarenessSceneObject> eventData)
        {
            RemoveFromData(eventData.Id);

            foreach (var sceneObjectDict in observedSceneObjects.Values)
            {
                sceneObjectDict?.Remove(eventData.Id);
            }
        }

        #endregion IMixedRealitySpatialAwarenessObservationHandler Implementations

        #region Public Functions

        /// <summary>
        /// Get all currently observed SceneObjects of a certain type.
        /// </summary>
        /// <remarks>
        /// Before calling this function, the observer should be configured to observe the specified type by including that type in the SurfaceTypes property.
        /// </remarks>
        /// <returns>A dictionary with the scene objects of the requested type being the values and their ids being the keys.</returns>
        public IReadOnlyDictionary<int, SpatialAwarenessSceneObject> GetSceneObjectsOfType(SpatialAwarenessSurfaceTypes type)
        {
            if (!observer.SurfaceTypes.IsMaskSet(type))
            {
                Debug.LogErrorFormat("The Scene Objects of type {0} are not being observed. You should add {0} to the SurfaceTypes property of the observer in advance.", type);
            }

            if (observedSceneObjects.TryGetValue(type, out Dictionary<int, SpatialAwarenessSceneObject> sceneObjects))
            {
                return sceneObjects;
            }
            else
            {
                observedSceneObjects.Add(type, new Dictionary<int, SpatialAwarenessSceneObject>());
                return observedSceneObjects[type];
            }
        }

        #region UI Functions

        public void UpdateScene()
        {
            observer.UpdateOnDemand();
        }

        public void SaveScene()
        {
            observer.SaveScene(SavedSceneNamePrefix);
        }

        public void ClearScene()
        {
            foreach (GameObject gameObject in instantiatedPrefabs)
            {
                Destroy(gameObject);
            }
            instantiatedPrefabs.Clear();
            observer.ClearObservations();
        }

        public void ToggleGeneratePlanes()
        {
            observer.RequestPlaneData = true;
            ClearAndUpdateObserver();
        }

        public void ToggleGenerateMeshes()
        {
            observer.RequestPlaneData = true;
            ClearAndUpdateObserver();
        }

        public void ToggleFloors()
        {
            ToggleObservedSurfaceType(SpatialAwarenessSurfaceTypes.Floor);
            ClearAndUpdateObserver();
        }

        public void ToggleWalls()
        {
            ToggleObservedSurfaceType(SpatialAwarenessSurfaceTypes.Wall);
            ClearAndUpdateObserver();
        }

        public void ToggleCeilings()
        {
            ToggleObservedSurfaceType(SpatialAwarenessSurfaceTypes.Ceiling);
            ClearAndUpdateObserver();
        }

        public void TogglePlatforms()
        {
            ToggleObservedSurfaceType(SpatialAwarenessSurfaceTypes.Platform);
            ClearAndUpdateObserver();
        }

        public void ToggleInferRegions()
        {
            observer.InferRegions = !observer.InferRegions;
            ClearAndUpdateObserver();
        }

        public void ToggleWorld()
        {
            ToggleObservedSurfaceType(SpatialAwarenessSurfaceTypes.World);

            if (observer.SurfaceTypes.IsMaskSet(SpatialAwarenessSurfaceTypes.World))
            {
                observer.RequestMeshData = true;
            }
            ClearAndUpdateObserver();
        }

        public void ToggleBackground()
        {
            ToggleObservedSurfaceType(SpatialAwarenessSurfaceTypes.Background);
            ClearAndUpdateObserver();
        }

        public void ToggleCompletelyInferred()
        {
            ToggleObservedSurfaceType(SpatialAwarenessSurfaceTypes.Inferred);
            ClearAndUpdateObserver();
        }

        #endregion UI Functions

        #endregion Public Functions

        #region Helper Functions

        private Color ColorForSurfaceType(SpatialAwarenessSurfaceTypes surfaceType)
        {
            switch (surfaceType)
            {
                case SpatialAwarenessSurfaceTypes.Unknown:
                    return new Color32(220, 50, 47, 255); // red
                case SpatialAwarenessSurfaceTypes.Floor:
                    return new Color32(38, 139, 210, 255); // blue
                case SpatialAwarenessSurfaceTypes.Ceiling:
                    return new Color32(108, 113, 196, 255); // violet
                case SpatialAwarenessSurfaceTypes.Wall:
                    return new Color32(181, 137, 0, 255); // yellow
                case SpatialAwarenessSurfaceTypes.Platform:
                    return new Color32(133, 153, 0, 255); // green
                case SpatialAwarenessSurfaceTypes.Background:
                    return new Color32(203, 75, 22, 255); // orange
                case SpatialAwarenessSurfaceTypes.World:
                    return new Color32(211, 54, 130, 255); // magenta
                case SpatialAwarenessSurfaceTypes.Inferred:
                    return new Color32(42, 161, 152, 255); // cyan
                default:
                    return new Color32(220, 50, 47, 255); // red
            }
        }

        private void ClearAndUpdateObserver()
        {
            ClearScene();
            observer.UpdateOnDemand();
        }

        private void ToggleObservedSurfaceType(SpatialAwarenessSurfaceTypes surfaceType)
        {
            if (observer.SurfaceTypes.IsMaskSet(surfaceType))
            {
                observer.SurfaceTypes &= ~surfaceType;
            }
            else
            {
                observer.SurfaceTypes |= surfaceType;
            }
        }

        void Update()
        {
            if (_mockPlanesScript != null)
            {
                foreach (var x in _mockPlanesScript.mockPlanes)
                {
                    EffectPlane foundPlane = _effectPlanesScript.effectPlanes.Find(f => f.id == x.id);
                    if (foundPlane)
                    {
                        // this is where we will pass the x/y size from HL2 plane to ours
                        foundPlane.updateSize(
                            Random.Range(0.1f, 0.2f),
                            Random.Range(0.1f, 0.2f)
                        );
                    }
                    else
                    {
                        //PlaneWatch planeW = gameObject.GetComponentInParent<PlaneWatch>();
                        EffectPlane newEffectPlane = _effectPlanesContainer.AddComponent<EffectPlane>();
                        newEffectPlane.id = x.id;
                        // newEffectPlane.plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        _effectPlanesScript.effectPlanes.Add(newEffectPlane);
                        Debug.Log("Added new effect plane");
                    }
                }
            }
            else
            {
                Debug.Log("mock planes script is null");
            }
        }

        #endregion Helper Functions
    }
}
