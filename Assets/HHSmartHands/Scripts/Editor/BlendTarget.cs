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

using UnityEditor;
using UnityEngine;


namespace DevonsToolbox {
namespace SmartHands {
    /// <summary>
    /// BlendTarget is a dataclass for assigning a Motion to a blend tree coordinate.
    /// It can technically be used anywhere, but in the context of SmartHands, it's mostly used for defining which 2D puppet thumbstick direction an animation corresponds to.
    /// </summary>
    public class BlendTarget {
        public BlendTarget(Vector2 position, Motion motion) {
            this.position = position;
            this.motion = motion;
        }

        public static BlendTarget FromAngle(float angleInDegrees, Motion motion) {
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
            return new BlendTarget(new Vector2(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians)), motion);
        }

        public Vector2 position;
        public Motion motion;
    }
}
}

#endif
