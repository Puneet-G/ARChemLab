//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
using HoloToolkit.Unity;
using HoloToolkit.Unity.Buttons;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.MRDL.PeriodicTable
{
    public class HighlightButton : Button
    {
        public override void OnStateChange(ButtonStateEnum newState)
        {
            MyButton element = gameObject.GetComponent<MyButton>();

            switch (newState)
            {
                case ButtonStateEnum.ObservationTargeted:
                case ButtonStateEnum.Targeted:
                    // If we're not the active element, light up
                    if (MyButton.ActiveElement != this)
                    {
                        element.Highlight();
                    }
                    break;

                default:
                    element.Dim();
                    break;
            }

            base.OnStateChange(newState);
        }
    }
}
