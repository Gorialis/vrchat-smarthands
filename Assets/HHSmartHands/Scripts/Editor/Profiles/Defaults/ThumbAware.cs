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
    public static class ThumbAware {
        public static GestureProfile fist = new BlendGestureProfile("Fist", true, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_ASL_S),
            BlendTarget.FromAngle(-90.0f, AssetCollection.sl_ASL_A),
            BlendTarget.FromAngle(-45.0f, AssetCollection.sl_ASL_T),
            BlendTarget.FromAngle(0.0f, AssetCollection.sl_ASL_N),
            BlendTarget.FromAngle(45.0f, AssetCollection.sl_ASL_M),
            BlendTarget.FromAngle(90.0f, AssetCollection.sl_ASL_E),
            BlendTarget.FromAngle(180.0f, AssetCollection.sl_ASL_FLAT_O),
        });

        public static GestureProfile openhand = new BlendGestureProfile("Open Hand", false, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_KSL_9),
            BlendTarget.FromAngle(90.0f, AssetCollection.sl_ASL_B),
            BlendTarget.FromAngle(45.0f, AssetCollection.sl_KSL_YE),
            BlendTarget.FromAngle(0.0f, AssetCollection.sl_KSL_RIEUL),
            BlendTarget.FromAngle(-45.0f, AssetCollection.sl_KSL_CHIEUCH),
            BlendTarget.FromAngle(-90.0f, AssetCollection.sl_ASL_9),
            BlendTarget.FromAngle(-135.0f, AssetCollection.sl_ASL_8),
            BlendTarget.FromAngle(-180.0f, AssetCollection.sl_ASL_7),
            BlendTarget.FromAngle(-225.0f, AssetCollection.sl_ASL_6),
        });

        public static GestureProfile point = new BlendGestureProfile("Point", false, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_ASL_D),
            BlendTarget.FromAngle(180.0f, AssetCollection.sl_ASL_X),
        });

        public static GestureProfile peace = new BlendGestureProfile("Peace", false, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_ASL_U),
            BlendTarget.FromAngle(-90.0f, AssetCollection.sl_ASL_R),
            BlendTarget.FromAngle(0.0f, AssetCollection.sl_ASL_V),
            BlendTarget.FromAngle(90.0f, AssetCollection.sl_ASL_K),
            BlendTarget.FromAngle(180.0f, AssetCollection.sl_KSL_MIEUM),
        });

        public static GestureProfile rocknroll = new BlendGestureProfile("Rock 'N' Roll", false, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_ASL_I),
            BlendTarget.FromAngle(-90.0f, AssetCollection.sl_ASL_Y),
            BlendTarget.FromAngle(-45.0f, AssetCollection.sl_ASL_ILY),
            BlendTarget.FromAngle(0.0f, AssetCollection.sl_KSL_YI),
        });

        public static GestureProfile fingergun = new BlendGestureProfile("Finger Gun", false, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_ASL_L),
            BlendTarget.FromAngle(-90.0f, AssetCollection.sl_BSL_C),
            BlendTarget.FromAngle(0.0f, AssetCollection.sl_KSL_CIEUC),
            BlendTarget.FromAngle(45.0f, AssetCollection.sl_KSL_KHIEUKH),
        });

        public static GestureProfile thumbsup = new BlendGestureProfile("Thumbs Up", false, new BlendTarget[] {
            new BlendTarget(new Vector2(0.0f, 0.0f), AssetCollection.sl_THUMBSUP),
            BlendTarget.FromAngle(90.0f, AssetCollection.sl_BSL_6),
        });

        public static HandProfile profile = new HandProfile("Thumb aware", new System.Tuple<int[], GestureProfile>[]{
            new System.Tuple<int[], GestureProfile>(new int[] {1}, fist),
            new System.Tuple<int[], GestureProfile>(new int[] {2}, openhand),
            new System.Tuple<int[], GestureProfile>(new int[] {3}, point),
            new System.Tuple<int[], GestureProfile>(new int[] {4}, peace),
            new System.Tuple<int[], GestureProfile>(new int[] {5}, rocknroll),
            new System.Tuple<int[], GestureProfile>(new int[] {6}, fingergun),
            new System.Tuple<int[], GestureProfile>(new int[] {7}, thumbsup),
        });
    }
}
}

#endif
