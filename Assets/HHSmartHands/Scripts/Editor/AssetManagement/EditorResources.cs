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

#if UNITY_EDITOR

using UnityEngine;


namespace DevonsToolbox {
    /// <summary>
    /// AssetCollection is a static class for retrieving correctly-typed assets for SmartHands in a RAII-style format.
    /// </summary>
    public static partial class AssetCollection {
        // Editor textures
        public static Texture banner { get {
            return GetAssetFallback<Texture>("8237cdc850c40a1408a679f1bf210b1e", "Assets/HHSmartHands/Textures/smart-hands-banner.png");
        }}

        public static Texture profileEditorBanner { get {
            return GetAssetFallback<Texture>("d2538d0d3a715b746b71f7ce7e4fafbf", "Assets/HHSmartHands/Textures/smart-hands-profile-editor-banner.png");
        }}

        public static Texture2D menuIcon { get {
            return GetAssetFallback<Texture2D>("bd8087cf0843abc45a9ea5050a84ee16", "Assets/HHSmartHands/Textures/hh-menu-icon.png");
        }}

        public static Texture2D menuIconNaive { get {
            return GetAssetFallback<Texture2D>("240dcbfd5039d7c459776e99305eeb32", "Assets/HHSmartHands/Textures/hh-menu-icon-naive.png");
        }}

        // Folders
        public static string artifactFolder { get {
            return GetAssetLocationFallback("bd61c977aa9dbd44bb2568a10fc2bfbe", "Assets/HHSmartHands/Artifacts");
        }}
        
    }
}

#endif
