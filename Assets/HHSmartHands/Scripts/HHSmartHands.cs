// -*- coding: utf-8 -*-
/*
MIT License

Copyright (c) 2021 Devon (Gorialis) R

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;
using System.Collections.Generic;
using UnityEngine;

#if VRC_SDK_VRCSDK3

using VRC.SDK3.Avatars.ScriptableObjects;

#endif


namespace DevonsToolbox {
namespace SmartHands {
    [AddComponentMenu("Devon's Toolbox/HH Smart Hands")]
    public class HHSmartHands : MonoBehaviour
    {
        public static string SMARTHANDS_VERSION = HHSmartHandsVersion.VersionString();

        [Serializable]
        public class GestureAssignments
        {
            public AnimationClip fist = null;
            public AnimationClip openHand = null;
            public AnimationClip point = null;
            public AnimationClip peace = null;
            public AnimationClip rockNRoll = null;
            public AnimationClip fingerGun = null;
            public AnimationClip thumbsUp = null;

            public Dictionary<int, AnimationClip> GetMapping() {
                return new Dictionary<int, AnimationClip> {
                    {1, fist},
                    {2, openHand},
                    {3, point},
                    {4, peace},
                    {5, rockNRoll},
                    {6, fingerGun},
                    {7, thumbsUp},
                };
            }
        }

        [Serializable]
        public class FoldoutSettings {
            public FoldoutSettings(string name, string tooltip = "") {
                this.name = name;
                this.tooltip = tooltip;
            }

            [NonSerialized]
            public string name;
            [NonSerialized]
            public string tooltip;

            public bool enabled = true;
            public bool foldout = false;
        }

        [Serializable]
        public class HandSettings : FoldoutSettings {
            public HandSettings(string name) : base(name, $"Smart mode for {name}") {

            }

            public int thumbAwareness = 0;
            public GestureAssignments assignments = new GestureAssignments();
        }

        [SerializeField]
        public HandSettings right = new HandSettings("Right Hand");

        [SerializeField]
        public HandSettings left = new HandSettings("Left Hand");

        [SerializeField]
        public GameObject avatar = null;

#if VRC_SDK_VRCSDK3

        [SerializeField]
        public VRCExpressionsMenu subMenu = null;

#endif
    }
}
}
