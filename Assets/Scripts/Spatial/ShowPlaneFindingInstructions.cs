// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Experimental.SpatialAwareness;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    public class ShowPlaneFindingInstructions : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The panel to display when the plane finding package is imported.")]
        private GameObject planeFindingPanel = null;

        void Start()
        {
            planeFindingPanel.SetActive(SurfaceMeshesToPlanes.CanCreatePlanes);
        }
    }
}
