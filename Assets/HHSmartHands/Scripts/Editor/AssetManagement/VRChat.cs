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

using VRC.SDK3.Avatars.ScriptableObjects;


namespace DevonsToolbox {
    /// <summary>
    /// AssetCollection is a static class for retrieving correctly-typed assets for SmartHands in a RAII-style format.
    /// </summary>
    public static partial class AssetCollection {
        // VRChat Sentinel Objects
        public static VRCExpressionParameters defaultParameters { get {
            return GetAssetFallback<VRCExpressionParameters>("81a6189720e21f6469c55c6256eb08dc", "Assets/HHSmartHands/Expressions/DefaultExpressionParameters.asset");
        }}

        // VRChat Animations
        public static AnimationClip idleProxy { get {
            return GetAssetFallback<AnimationClip>("14980fc5fe40191418954549174fe63e", "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_hands_idle.anim");
        }}

        public static AnimationClip idle2Proxy { get {
            return GetAssetFallback<AnimationClip>("61a99b5de5e4b6d4c8ed51d9dfd9ddc7", "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_hands_idle2.anim");
        }}

        public static AnimationClip fistProxy { get {
            return GetAssetFallback<AnimationClip>("523de46ec8739104f91a2b54fa49cdc7", "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_hands_fist.anim");
        }}

        public static AnimationClip openhandProxy { get {
            return GetAssetFallback<AnimationClip>("e519e4ad96b4b4b49901f99adce46a64", "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_hands_open.anim");
        }}

        public static AnimationClip pointProxy { get {
            return GetAssetFallback<AnimationClip>("db055938a2cca0849b43d69957171c7a", "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_hands_point.anim");
        }}

        public static AnimationClip peaceProxy { get {
            return GetAssetFallback<AnimationClip>("c24dee443c8cd15498f706a6571d400f", "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_hands_peace.anim");
        }}

        public static AnimationClip rocknrollProxy { get {
            return GetAssetFallback<AnimationClip>("219afa41c622312419dc6ac65e3657c3", "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_hands_rock.anim");
        }}

        public static AnimationClip fingergunProxy { get {
            return GetAssetFallback<AnimationClip>("fe8651e0359eacb49af5f71cc04eadd5", "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_hands_gun.anim");
        }}

        public static AnimationClip thumbsupProxy { get {
            return GetAssetFallback<AnimationClip>("9bad171d3023a114c8f42ea671be2af4", "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_hands_thumbs_up.anim");
        }}

        // VRChat Masks
        public static AvatarMask handsOnlyMask { get {
            return GetAssetFallback<AvatarMask>("b2b8bad9583e56a46a3e21795e96ad92", "Assets/VRCSDK/Examples3/Animation/Masks/vrc_HandsOnly.mask");
        }}

        public static AvatarMask leftHandMask { get {
            return GetAssetFallback<AvatarMask>("7ff0199655202a04eb175de45a6e078a", "Assets/VRCSDK/Examples3/Animation/Masks/vrc_Hand Left.mask");
        }}

        public static AvatarMask rightHandMask { get {
            return GetAssetFallback<AvatarMask>("903ce375d5f609d44b9f00b425d6eda9", "Assets/VRCSDK/Examples3/Animation/Masks/vrc_Hand Right.mask");
        }}
    }
}

#endif
