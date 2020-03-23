//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HoloToolkit.MRDL.PeriodicTable
{
    public class MyButton : MonoBehaviour
    {
        public static MyButton ActiveElement;
        public Renderer BoxRenderer;
        public MeshRenderer PanelFront;
        public MeshRenderer PanelBack;
        public MeshRenderer PanelLeft;
        public MeshRenderer PanelRight;
        private BoxCollider boxCollider;
        private Color origColor;

        public void Start()
        {
            // Turn off our animator until it's needed
            GetComponent<Animator>().enabled = false;
            BoxRenderer.enabled = true;
            origColor = PanelBack.sharedMaterial.color;
        }

        public void Highlight()
        {
            if (ActiveElement == this)
                return;

            
            PanelBack.sharedMaterial.color *= 1.1f;
            PanelFront.sharedMaterial.color *= 1.1f;
            PanelRight.sharedMaterial.color *= 1.1f;
            PanelLeft.sharedMaterial.color *= 1.1f;
            BoxRenderer.sharedMaterial.color *= 1.1f;
            
        }

        public void Dim()
        {
            if (ActiveElement == this)
                return;
            string parentName = PanelBack.transform.parent.parent.name;

            PanelBack.sharedMaterial.color = origColor;
            PanelFront.sharedMaterial.color = origColor;
            PanelLeft.sharedMaterial.color = origColor;
            PanelRight.sharedMaterial.color = origColor;
            BoxRenderer.sharedMaterial.color = origColor;

        }
    }
}
