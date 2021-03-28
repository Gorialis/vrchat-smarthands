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

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;


namespace DevonsToolbox {
namespace SmartHands {
    /// <summary>
    /// A gesture profile is a profile that encompasses one gesture (such as fist), in a given mode.
    /// For instance, one could have a VICTORY gesture profile where the default is V-hand, and another where the default is U-hand.
    /// Most broadly, a gesture profile encompasses the puppet for a gesture.
    /// Users can create their own gestures profiles to assign custom puppet behaviors to a gesture.
    /// There are also stock gesture profiles which are 'unmodifiable'.
    /// </summary>
    public abstract class GestureProfile {
        public GestureProfile(string name, bool useGestureWeight) {
            this.name = name;
            this.useGestureWeight = useGestureWeight;
        }

        public abstract Motion CreateMotion(AnimatorController controller, HandConfigurations.Configuration config);
        public AnimatorState CreateState(AnimatorController controller, HandConfigurations.Configuration config) {
            AnimatorState state = new AnimatorState();
            state.hideFlags = HideFlags.HideInHierarchy;
            state.name = $"{name}";
            AssetDatabase.AddObjectToAsset(state, controller);

            Motion motion = CreateMotion(controller, config);

            if (useGestureWeight) {
                // If this uses gesture weight, make a blend tree to ease it with the idle hand
                BlendTree blendTree = new BlendTree();

                blendTree.name = $"{name} Weighting Tree";

                blendTree.blendType = BlendTreeType.Simple1D;
                blendTree.blendParameter = config.gestureWeightParameter;

                blendTree.hideFlags = HideFlags.HideInHierarchy;

                blendTree.AddChild(AssetCollection.idleProxy, new Vector2(0.0f, 0.0f));
                blendTree.AddChild(motion, new Vector2(1.0f, 0.0f));

                state.motion = blendTree;
                AssetDatabase.AddObjectToAsset(blendTree, controller);
            } else {
                state.motion = motion;
            }

            return state;
        }

        public string name = "Gesture profile";
        public bool useGestureWeight = false;
        
    }

    public class SimpleGestureProfile : GestureProfile {
        public SimpleGestureProfile(string name, bool useGestureWeight, Motion motion) : base(name, useGestureWeight) {
            this.motion = motion;
        }

        public override Motion CreateMotion(AnimatorController controller, HandConfigurations.Configuration config) {
            return motion;
        }

        public Motion motion;
    }

    public class BlendGestureProfile : GestureProfile {
        public BlendGestureProfile(string name, bool useGestureWeight, BlendTarget[] targets) : base(name, useGestureWeight) {
            this.targets = targets;
        }

        public override Motion CreateMotion(AnimatorController controller, HandConfigurations.Configuration config) {
            BlendTree blendTree = new BlendTree();

            blendTree.name = $"{name} Tree";

            blendTree.blendType = BlendTreeType.FreeformDirectional2D;
            blendTree.blendParameter = config.poseXParameter;
            blendTree.blendParameterY = config.poseYParameter;

            blendTree.hideFlags = HideFlags.HideInHierarchy;

            foreach (BlendTarget target in targets) {
                blendTree.AddChild(target.motion, target.position * config.transform);
            }

            AssetDatabase.AddObjectToAsset(blendTree, controller);
            return blendTree;
        }

        public BlendTarget[] targets;
    }
}
}

#endif
