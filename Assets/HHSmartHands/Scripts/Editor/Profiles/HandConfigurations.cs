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

#if (UNITY_EDITOR && VRC_SDK_VRCSDK3)

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;


namespace DevonsToolbox {
namespace SmartHands {
    /// <summary>
    ///
    /// </summary>
    public static class HandConfigurations {
        /// <summary>
        /// HandConfigurations.Configuration is a class that encapsulates information about each hand configuration for a target avatar.
        /// Generally speaking, I'd hope most avatars only have two hands. For this reason, you might consider this class a bit of a redundant modularization.
        /// The main reason it exists is to simplify code elsewhere, where most of the control flow is identical between hands, with only the referent identifiers changing.
        /// </summary>
        public class Configuration {
            public Configuration(string name, AvatarMask mask, Vector2 transform) {
                this.name = name;
                this.mask = mask;
                this.transform = transform;
            }

            public string name;
            public AvatarMask mask;
            public Vector2 transform;

            public string gestureIndexParameter { get { return $"Gesture{name}"; }}
            public string gestureWeightParameter { get { return $"Gesture{name}Weight"; }}
            public string poseXParameter { get { return $"SmartHands{name}X"; }}
            public string poseYParameter { get { return $"SmartHands{name}Y"; }}
            public string poseModeParameter { get { return $"SmartHands{name}Mode"; }}
        }

        public static Configuration left = new Configuration("Left", AssetCollection.leftHandMask, new Vector2(-1.0f, 1.0f));
        public static Configuration right = new Configuration("Right", AssetCollection.rightHandMask, new Vector2(1.0f, 1.0f));
    }
}
}

#endif
