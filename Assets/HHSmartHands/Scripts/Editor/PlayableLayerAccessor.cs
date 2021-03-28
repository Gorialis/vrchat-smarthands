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

using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace DevonsToolbox {
namespace SmartHands {
    public class PlayableLayerAccessor {
        /// <summary>
        /// PlayableLayerAccessor unpacks the animation layer arrays from a descriptor into named get/set properties for each layer.
        /// I honestly don't know why they're not just properties in the first place, but this vastly simplifies the code you have to write to deal with playable layers therefore.
        /// </summary>
        public PlayableLayerAccessor(VRCAvatarDescriptor descriptor) {
            this.descriptor = descriptor;

            for (int i = 0; i < descriptor.baseAnimationLayers.Length; ++i) {
                VRCAvatarDescriptor.CustomAnimLayer layer = descriptor.baseAnimationLayers[i];

                switch (layer.type) {
                    case VRCAvatarDescriptor.AnimLayerType.Base:
                        baseLayerIndex = i;
                        break;
                    case VRCAvatarDescriptor.AnimLayerType.Additive:
                        additiveLayerIndex = i;
                        break;
                    case VRCAvatarDescriptor.AnimLayerType.Gesture:
                        gestureLayerIndex = i;
                        break;
                    case VRCAvatarDescriptor.AnimLayerType.Action:
                        actionLayerIndex = i;
                        break;
                    case VRCAvatarDescriptor.AnimLayerType.FX:
                        fxLayerIndex = i;
                        break;
                    default:
                        Debug.Log($"Unknown base layer type in DevonsToolbox.PlayableLayerAccessor: {layer.type}");
                        break;
                }
            }

            for (int i = 0; i < descriptor.specialAnimationLayers.Length; ++i) {
                VRCAvatarDescriptor.CustomAnimLayer layer = descriptor.specialAnimationLayers[i];

                switch (layer.type) {
                    case VRCAvatarDescriptor.AnimLayerType.Sitting:
                        sittingLayerIndex = i;
                        break;
                    case VRCAvatarDescriptor.AnimLayerType.TPose:
                        tposeLayerIndex = i;
                        break;
                    case VRCAvatarDescriptor.AnimLayerType.IKPose:
                        ikposeLayerIndex = i;
                        break;
                    default:
                        Debug.Log($"Unknown special layer type in DevonsToolbox.PlayableLayerAccessor: {layer.type}");
                        break;
                }
            }
        }

        /// <summary>
        /// This is a shim that automatically sets the proper attributes for assignment of a controller to a playable layer.
        /// </summary>
        public static VRCAvatarDescriptor.CustomAnimLayer Set(VRCAvatarDescriptor.CustomAnimLayer layer, RuntimeAnimatorController controller) {
            layer.isDefault = false;
            layer.animatorController = controller;
            
            return layer;
        }

        /// <summary>
        /// This is a shim that automatically sets the proper attributes for clearing a playable layer.
        /// </summary>
        public static VRCAvatarDescriptor.CustomAnimLayer Clear(VRCAvatarDescriptor.CustomAnimLayer layer) {
            layer.isDefault = true;
            layer.animatorController = null;

            return layer;
        }

        VRCAvatarDescriptor descriptor;

        // Base layers
        public int baseLayerIndex;
        public int additiveLayerIndex;
        public int gestureLayerIndex;
        public int actionLayerIndex;
        public int fxLayerIndex;
        public VRCAvatarDescriptor.CustomAnimLayer baseLayer { get { return descriptor.baseAnimationLayers[baseLayerIndex]; } set { descriptor.baseAnimationLayers[baseLayerIndex] = value; } }
        public VRCAvatarDescriptor.CustomAnimLayer additiveLayer { get { return descriptor.baseAnimationLayers[additiveLayerIndex]; } set { descriptor.baseAnimationLayers[additiveLayerIndex] = value; } }
        public VRCAvatarDescriptor.CustomAnimLayer gestureLayer { get { return descriptor.baseAnimationLayers[gestureLayerIndex]; } set { descriptor.baseAnimationLayers[gestureLayerIndex] = value; } }
        public VRCAvatarDescriptor.CustomAnimLayer actionLayer { get { return descriptor.baseAnimationLayers[actionLayerIndex]; } set { descriptor.baseAnimationLayers[actionLayerIndex] = value; } }
        public VRCAvatarDescriptor.CustomAnimLayer fxLayer { get { return descriptor.baseAnimationLayers[fxLayerIndex]; } set { descriptor.baseAnimationLayers[fxLayerIndex] = value; } }

        // Special layers
        public int sittingLayerIndex;
        public int tposeLayerIndex;
        public int ikposeLayerIndex;
        public VRCAvatarDescriptor.CustomAnimLayer sittingLayer { get { return descriptor.specialAnimationLayers[sittingLayerIndex]; } set { descriptor.specialAnimationLayers[sittingLayerIndex] = value; } }
        public VRCAvatarDescriptor.CustomAnimLayer tposeLayer { get { return descriptor.specialAnimationLayers[tposeLayerIndex]; } set { descriptor.specialAnimationLayers[tposeLayerIndex] = value; } }
        public VRCAvatarDescriptor.CustomAnimLayer ikposeLayer { get { return descriptor.specialAnimationLayers[ikposeLayerIndex]; } set { descriptor.specialAnimationLayers[ikposeLayerIndex] = value; } }
    }
}
}

#endif
