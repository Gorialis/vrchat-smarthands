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
    /// A hand profile is a profile that encompasses an entire hand's response to gestures in a given mode.
    /// For instance, thumb-aware is one hand profile, and thumb-naive is another.
    /// Most broadly, a hand profile maps a GESTURE ID to a GESTURE PROFILE.
    /// Users can create their own hand profiles to assign any gesture profile to any gesture ID they want.
    /// There are also stock hand profiles which are 'unmodifiable'.
    /// </summary>
    public class HandProfile {
        public HandProfile(string name, Tuple<int[], GestureProfile>[] assignments) {
            this.name = name;
            this.assignments = assignments;
        }

        public string name;
        public Tuple<int[], GestureProfile>[] assignments;
    }

    public static class HandProfileCollection {
        public static HandProfile[] all = {
            Nonsmart.profile,
            ThumbAware.profile,
            ThumbNaive.profile
        };
    }
}
}

#endif
