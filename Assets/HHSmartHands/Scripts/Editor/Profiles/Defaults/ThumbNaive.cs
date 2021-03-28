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
    public static class ThumbNaive {
        public static GestureProfile fist_thumbsup = new BlendGestureProfile("Fist / Thumbs Up", false, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_ASL_S),
            BlendTarget.FromAngle(-90.0f, AssetCollection.sl_THUMBSUP),
            BlendTarget.FromAngle(-45.0f, AssetCollection.sl_ASL_A),
            BlendTarget.FromAngle(0.0f, AssetCollection.sl_ASL_T),
            BlendTarget.FromAngle(45.0f, AssetCollection.sl_ASL_N),
            BlendTarget.FromAngle(90.0f, AssetCollection.sl_ASL_M),
            BlendTarget.FromAngle(135.0f, AssetCollection.sl_ASL_E),
            BlendTarget.FromAngle(180.0f, AssetCollection.sl_ASL_FLAT_O),
        });

        public static GestureProfile openhand_peace = new BlendGestureProfile("Open Hand / Peace", true, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_KSL_9),
            BlendTarget.FromAngle(90.0f, AssetCollection.sl_ASL_B),
            BlendTarget.FromAngle(60.0f, AssetCollection.sl_KSL_YE),

            BlendTarget.FromAngle(30.0f, AssetCollection.sl_ASL_K),
            new BlendTarget(new Vector2(0.0f, 0.5f), AssetCollection.sl_ASL_U),
            BlendTarget.FromAngle(0.0f, AssetCollection.sl_ASL_V),
            BlendTarget.FromAngle(-30.0f, AssetCollection.sl_ASL_R),

            BlendTarget.FromAngle(-60.0f, AssetCollection.sl_KSL_RIEUL),
            BlendTarget.FromAngle(-90.0f, AssetCollection.sl_KSL_CHIEUCH),
            BlendTarget.FromAngle(-150.0f, AssetCollection.sl_ASL_9),
            BlendTarget.FromAngle(-165.0f, AssetCollection.sl_ASL_8),
            BlendTarget.FromAngle(-195.0f, AssetCollection.sl_ASL_7),
            BlendTarget.FromAngle(-210.0f, AssetCollection.sl_ASL_6),
        });

        public static GestureProfile point_fingergun = new BlendGestureProfile("Point / Finger Gun", false, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_ASL_L),
            BlendTarget.FromAngle(-90.0f, AssetCollection.sl_BSL_C),
            BlendTarget.FromAngle(0.0f, AssetCollection.sl_KSL_CIEUC),
            BlendTarget.FromAngle(45.0f, AssetCollection.sl_KSL_KHIEUKH),
            BlendTarget.FromAngle(90.0f, AssetCollection.sl_ASL_D),
            BlendTarget.FromAngle(180.0f, AssetCollection.sl_ASL_X),
        });

        public static HandProfile profile = new HandProfile("Thumb naive", new System.Tuple<int[], GestureProfile>[]{
            new System.Tuple<int[], GestureProfile>(new int[] {1, 7}, fist_thumbsup),
            new System.Tuple<int[], GestureProfile>(new int[] {2, 4}, openhand_peace),
            new System.Tuple<int[], GestureProfile>(new int[] {3, 6}, point_fingergun),
            new System.Tuple<int[], GestureProfile>(new int[] {5}, ThumbAware.rocknroll),
        });
    }
}
}

#endif
