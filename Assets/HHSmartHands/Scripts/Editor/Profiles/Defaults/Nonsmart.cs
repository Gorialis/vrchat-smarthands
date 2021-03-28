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
    public static class Nonsmart {
        public static GestureProfile fist = new SimpleGestureProfile("Fist", true, AssetCollection.sl_ASL_A);

        public static GestureProfile openhand = new SimpleGestureProfile("Open Hand", false, AssetCollection.sl_KSL_9);

        public static GestureProfile point = new SimpleGestureProfile("Point", false, AssetCollection.sl_ASL_D);

        public static GestureProfile peace = new SimpleGestureProfile("Peace", false, AssetCollection.sl_ASL_V);

        public static GestureProfile rocknroll = new SimpleGestureProfile("Rock 'N' Roll", false, AssetCollection.sl_ASL_ILY);

        public static GestureProfile fingergun = new SimpleGestureProfile("Finger Gun", false, AssetCollection.sl_ASL_L);

        public static GestureProfile thumbsup = new SimpleGestureProfile("Thumbs Up", false, AssetCollection.sl_THUMBSUP);

        public static HandProfile profile = new HandProfile("Non-smart", new System.Tuple<int[], GestureProfile>[]{
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
