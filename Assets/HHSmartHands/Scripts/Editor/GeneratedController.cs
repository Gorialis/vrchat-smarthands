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
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;


namespace DevonsToolbox {
namespace SmartHands {
    /// <summary>
    /// GeneratedController is a RAII class for generating the SmartHands animator controllers based off the serialized script settings.
    /// The <value>controller</value> it generates is a VRCSDK3A-compatible Gesture/Hands layer.
    /// </summary>
    class GeneratedController {
        public GeneratedController(string path, HHSmartHands settings) {
            controller = AnimatorController.CreateAnimatorControllerAtPath(path);

            // Set up the default AllParts layer
            AnimatorControllerLayer[] layers = controller.layers;
            layers[0].name = "AllParts";
            layers[0].avatarMask = AssetCollection.handsOnlyMask;
            controller.layers = layers;

            var pairs = new System.Tuple<HandConfigurations.Configuration, HHSmartHands.GestureAssignments>[] {
                new System.Tuple<HandConfigurations.Configuration, HHSmartHands.GestureAssignments> (HandConfigurations.left, settings.left.assignments),
                new System.Tuple<HandConfigurations.Configuration, HHSmartHands.GestureAssignments> (HandConfigurations.right, settings.right.assignments),
            };

            foreach (var pair in pairs) {
                var configuration = pair.Item1;

                AddParameter(controller, configuration.gestureIndexParameter, AnimatorControllerParameterType.Int);
                AddParameter(controller, configuration.gestureWeightParameter, AnimatorControllerParameterType.Float);

                AddParameter(controller, configuration.poseXParameter, AnimatorControllerParameterType.Float);
                AddParameter(controller, configuration.poseYParameter, AnimatorControllerParameterType.Float);
                AddParameter(controller, configuration.poseModeParameter, AnimatorControllerParameterType.Int);

                controller.AddLayer(GenerateGestureLayer(controller, configuration, pair.Item2));
            }

            // Ensure controller changes get saved
            EditorUtility.SetDirty(controller);
        }

        public GeneratedController(AnimatorController controller, HHSmartHands settings) {
            this.controller = controller;

            var pairs = new System.Tuple<HandConfigurations.Configuration, HHSmartHands.GestureAssignments>[] {
                new System.Tuple<HandConfigurations.Configuration, HHSmartHands.GestureAssignments> (HandConfigurations.left, settings.left.assignments),
                new System.Tuple<HandConfigurations.Configuration, HHSmartHands.GestureAssignments> (HandConfigurations.right, settings.right.assignments),
            };

            // Remove old layers
            // Get controller layers
            List<AnimatorControllerLayer> layerList = controller.layers.ToList();

            List<string> smartHandsLayerNames = new List<string>();

            foreach (var pair in pairs) {
                smartHandsLayerNames.Add($"SmartHands {pair.Item1.name} Hand");
            }

            // Delete SmartHands layers if they are already present
            layerList.RemoveAll(l => smartHandsLayerNames.Contains(l.name));

            foreach (var pair in pairs) {
                var configuration = pair.Item1;

                AddParameter(controller, configuration.gestureIndexParameter, AnimatorControllerParameterType.Int);
                AddParameter(controller, configuration.gestureWeightParameter, AnimatorControllerParameterType.Float);

                AddParameter(controller, configuration.poseXParameter, AnimatorControllerParameterType.Float);
                AddParameter(controller, configuration.poseYParameter, AnimatorControllerParameterType.Float);
                AddParameter(controller, configuration.poseModeParameter, AnimatorControllerParameterType.Int);

                layerList.Add(GenerateGestureLayer(controller, configuration, pair.Item2));
            }

            // Assign back layers
            controller.layers = layerList.ToArray();

            // Ensure controller changes get saved
            EditorUtility.SetDirty(controller);
        }

        private void AddParameter(AnimatorController controller, string name, AnimatorControllerParameterType parameterType) {
            foreach (AnimatorControllerParameter parameter in controller.parameters) {
                if (parameter.name == name)
                    return;
            }

            controller.AddParameter(name, parameterType);
        }

        private AnimatorControllerLayer GenerateGestureLayer(AnimatorController parent, HandConfigurations.Configuration config, HHSmartHands.GestureAssignments assignments) {
            AnimatorControllerLayer layer = new AnimatorControllerLayer();
            layer.name = $"SmartHands {config.name} Hand";
            layer.avatarMask = config.mask;
            layer.defaultWeight = 1.0f;

            AnimatorStateMachine fsm = new AnimatorStateMachine();
            fsm.name = layer.name;
            fsm.hideFlags = HideFlags.HideInHierarchy;
            layer.stateMachine = fsm;

            AssetDatabase.AddObjectToAsset(fsm, parent);

            // Clean up visual positioning
            fsm.entryPosition = new Vector3(0, 60, 0);
            fsm.exitPosition = new Vector3(622, 60, 0);
            fsm.anyStatePosition = new Vector3(322, 300, 0);

            // Create states
            AnimatorState idle = fsm.AddState("Idle", new Vector3(300, 60, 0));
            fsm.defaultState = idle;

            // Enable finger tracking on this state
            VRCAnimatorTrackingControl idleCtrl = idle.AddStateMachineBehaviour(typeof(VRCAnimatorTrackingControl)) as VRCAnimatorTrackingControl;

            if (config.transform.x < 0.0f)
                idleCtrl.trackingLeftFingers = VRCAnimatorTrackingControl.TrackingType.Tracking;
            else
                idleCtrl.trackingRightFingers = VRCAnimatorTrackingControl.TrackingType.Tracking;

            AnimatorStateTransition idleTransition = fsm.AddAnyStateTransition(idle);
            idleTransition.AddCondition(AnimatorConditionMode.Equals, 0, config.gestureIndexParameter);
            idleTransition.duration = 0.1f;

            HandProfile[] handProfiles = HandProfileCollection.all;
            var defaultOverrides = assignments.GetMapping();

            for (int handProfileIndex = 0; handProfileIndex < handProfiles.Length; ++handProfileIndex) {
                HandProfile handProfile = handProfiles[handProfileIndex];

                for (int gestureProfileIndex = 0; gestureProfileIndex < handProfile.assignments.Length; ++gestureProfileIndex) {
                    var assignmentPair = handProfile.assignments[gestureProfileIndex];

                    GestureProfile gestureProfile = assignmentPair.Item2;
                    AnimationClip potentialOverride;

                    if (handProfileIndex == 0 && assignmentPair.Item1.Length == 1 && defaultOverrides.TryGetValue(assignmentPair.Item1[0], out potentialOverride) && potentialOverride != null) {
                        // Override for default non-smart
                        gestureProfile = new SimpleGestureProfile(gestureProfile.name, gestureProfile.useGestureWeight, potentialOverride);
                    }

                    AnimatorState state = gestureProfile.CreateState(parent, config);
                    state.name = fsm.MakeUniqueStateName($"{state.name} ({handProfile.name})");
                    fsm.AddState(state, new Vector3(300 * (handProfileIndex + 2), 60 * (gestureProfileIndex + 2), 0.0f));
                    
                    VRCAnimatorTrackingControl ctrl = state.AddStateMachineBehaviour(typeof(VRCAnimatorTrackingControl)) as VRCAnimatorTrackingControl;

                    var trackingType = handProfileIndex == 0 ? VRCAnimatorTrackingControl.TrackingType.Tracking : VRCAnimatorTrackingControl.TrackingType.Animation;

                    if (config.transform.x < 0.0f)
                        ctrl.trackingLeftFingers = trackingType;
                    else
                        ctrl.trackingRightFingers = trackingType;

                    foreach (int transitionIndex in assignmentPair.Item1) {
                        AnimatorStateTransition smartTransition = fsm.AddAnyStateTransition(state);
                        smartTransition.AddCondition(AnimatorConditionMode.Equals, transitionIndex, config.gestureIndexParameter);
                        smartTransition.AddCondition(AnimatorConditionMode.Equals, handProfileIndex, config.poseModeParameter);
                        smartTransition.duration = 0.1f;
                    }
                }
            }

            return layer;
        }

        public AnimatorController controller;
    }
}
}

#endif
