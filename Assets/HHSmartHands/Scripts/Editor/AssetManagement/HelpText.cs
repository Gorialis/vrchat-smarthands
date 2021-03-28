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

#if (UNITY_EDITOR)

namespace DevonsToolbox
{
    public static class HelpText {
        public static string thumbAwareness = "SmartHands uses the Avatar 3.0 two-axis puppets to extend the set of gestures on an avatar through use of the thumbstick.\n\nThis works fine and operates well when using one hand (such as the left hand) to extend the gestures of the other (such as the right hand).\nHowever, if the hand doing the puppeting is the same as the one that is being puppetted, such as if you are using SmartHands on both hands, this creates a problem.\n\nCertain gestures, such as THUMBSUP, cannot be controlled properly because the usage of the thumbstick means the thumb cannot be up.\n\nTo address this problem for two-handed usage, SmartHands has a 'Thumb-Naive' mode where certain gestures, such as FIST and THUMBSUP, are combined into a single puppet such that the state of the thumb does not matter (SmartHands is 'naive' of the thumb state) - almost all hand shapes will still be possible even if they would usually require the thumb to be up.\n\nThis does not solve all cases - OPENHAND still cannot be puppetted on Index with one hand because VRChat resolves this controller state to Idle instead, which is indistinguishable from an invalid gesture, but it does make two-handed signing more plausible in general.\n\nYou can choose to generate Thumb-Aware or Thumb-Naive states, or both. SmartHands will always generate with support for both - this setting only changes what you see in your menu in-game.";
    }
}

#endif
