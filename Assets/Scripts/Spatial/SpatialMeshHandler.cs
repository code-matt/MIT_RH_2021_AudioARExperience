using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using System.Collections.Generic;
using UnityEngine;

namespace ASV
{
    using SpatialAwarenessHandler = IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessMeshObject>;

    public class SpatialMeshHandler : MonoBehaviour, SpatialAwarenessHandler
    {
        private Dictionary<int, uint> meshUpdateData = new Dictionary<int, uint>();

        protected bool isRegistered = false;

        protected virtual void Start()
        {
            RegisterEventHandlers<SpatialAwarenessHandler, SpatialAwarenessMeshObject>();
        }

        protected virtual void OnEnable()
        {
            RegisterEventHandlers<SpatialAwarenessHandler, SpatialAwarenessMeshObject>();
        }

        protected virtual void OnDisable()
        {
            UnregisterEventHandlers<SpatialAwarenessHandler, SpatialAwarenessMeshObject>();
        }

        protected virtual void OnDestroy()
        {
            UnregisterEventHandlers<SpatialAwarenessHandler, SpatialAwarenessMeshObject>();
        }

        protected virtual void RegisterEventHandlers<T, U>()
            where T : IMixedRealitySpatialAwarenessObservationHandler<U>
            where U : BaseSpatialAwarenessObject
        {
            if (!isRegistered && (CoreServices.SpatialAwarenessSystem != null))
            {
                CoreServices.SpatialAwarenessSystem.RegisterHandler<T>(this);
                isRegistered = true;
            }
        }

        protected virtual void UnregisterEventHandlers<T, U>()
            where T : IMixedRealitySpatialAwarenessObservationHandler<U>
            where U : BaseSpatialAwarenessObject
        {
            if (isRegistered && (CoreServices.SpatialAwarenessSystem != null))
            {
                CoreServices.SpatialAwarenessSystem.UnregisterHandler<T>(this);
                isRegistered = false;
            }
        }

        public virtual void OnObservationAdded(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
        {
            AddToData(eventData.Id);
        }

        public virtual void OnObservationUpdated(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
        {
            UpdateData(eventData.Id);
        }

        public virtual void OnObservationRemoved(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
        {
            RemoveFromData(eventData.Id);
        }

        protected void AddToData(int eventDataId)
        {
            Debug.Log($"Tracking mesh {eventDataId}");
            meshUpdateData.Add(eventDataId, 0);
        }

        protected void UpdateData(int eventDataId)
        {
            // A mesh has been updated. Find it and increment the update count.
            if (meshUpdateData.TryGetValue(eventDataId, out uint updateCount))
            {
                // Set the new update count.
                meshUpdateData[eventDataId] = ++updateCount;

                Debug.Log($"Mesh {eventDataId} has been updated {updateCount} times.");
            }
        }

        protected void RemoveFromData(int eventDataId)
        {
            // A mesh has been removed. We no longer need to track the count of updates.
            if (meshUpdateData.ContainsKey(eventDataId))
            {
                Debug.Log($"No longer tracking mesh {eventDataId}.");
                meshUpdateData.Remove(eventDataId);
            }
        }
    }
}
