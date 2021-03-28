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

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

#if VRC_SDK_VRCSDK3

using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace DevonsToolbox {
namespace SmartHands {
    [CustomEditor(typeof(HHSmartHands))]
    public class HHSmartHandsEditor : Editor
    {
        /// <summary>
        /// Checks if a given object is associated with an avatar - that is, the object would be uploaded as part of that avatar.
        /// </summary>
        /// <remarks>Constituted by a check for a VRCAvatarDescriptor component on the object and its hierarchical parents.</remarks>
        /// <returns><c>true</c> if the object is associated with an avatar, <c>false</c> otherwise.</returns>
        public static bool IsOnAvatar(GameObject obj) {
            GameObject testObject = obj;

            if (testObject.GetComponent<VRCAvatarDescriptor>() != null) return true;

            while (testObject.transform.parent != null) {
                testObject = testObject.transform.parent.gameObject;

                if (testObject.GetComponent<VRCAvatarDescriptor>() != null) return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the menu specified by <paramref name="child"/> is one of the children of the menu specified by <paramref name="parent"/>.
        /// This means **ALL** children, not just immediate children, thus this function may recurse.
        /// </summary>
        /// <param name="parent">The parent menu to check the children of.</param>
        /// <param name="child">The menu that is to be asserted a child or not.</param>
        /// <param name="searchHistory">A history of the menus already checked as part of the tree search from the parent. Usually only useful internally, leave it null if you're not sure what you're doing.</param>
        /// <remarks>The search history is used to prevent issues with circular references. If menu A specifies menu B as a submenu, and menu B specified menu A as a submenu, the search history prevents this function from getting stuck in an infinite recursive loop.</remarks>
        /// <returns><c>true</c> if the child menu is one of the children of the parent menu, <c>false</c> otherwise.</returns>
        public static bool IsMenuChildOf(VRCExpressionsMenu parent, VRCExpressionsMenu child, List<VRCExpressionsMenu> searchHistory = null) {
            if (searchHistory == null)
                searchHistory = new List<VRCExpressionsMenu>();

            searchHistory.Add(parent);

            foreach (VRCExpressionsMenu.Control control in parent.controls)
            {
                if (control.type == VRCExpressionsMenu.Control.ControlType.SubMenu) {
                    if (control.subMenu == child)
                        return true;
                    if (!searchHistory.Contains(control.subMenu) && IsMenuChildOf(control.subMenu, child))
                        return true;
                }
            }

            return false;
        }

        private void DrawDropdown(HHSmartHands.FoldoutSettings settings) {
            GUIStyle style = new GUIStyle("ShurikenModuleTitle");
            style.font = new GUIStyle(EditorStyles.boldLabel).font;
            style.border = new RectOffset(15, 7, 4, 4);
            style.fixedHeight = 22;
            style.contentOffset = new Vector2(20f, -2f);

            Rect rect = GUILayoutUtility.GetRect(16f + 20f, 22f, style);
            GUI.Box(rect, new GUIContent("     " + settings.name, settings.tooltip), style);

            Rect toggleRect = new Rect(rect);
            toggleRect.x += 20f;
            toggleRect.y += 2f;
            toggleRect.width = 15f;
            toggleRect.height = 15f;

            settings.enabled = EditorGUI.Toggle(toggleRect, settings.enabled);

            Event e = Event.current;
            switch (e.type) {
                case EventType.Repaint:
                    Rect foldoutRect = new Rect(rect.x + 4f, rect.y + 3f, 13f, 13f);
                    EditorStyles.foldout.Draw(foldoutRect, false, false, settings.foldout, false);
                    break;
                case EventType.MouseDown:
                    if (rect.Contains(e.mousePosition) && !e.alt) {
                        settings.foldout = !settings.foldout;
                        e.Use();
                    }
                    break;

            }
        }

        private void DrawSettings(HHSmartHands.HandSettings settings) {
            DrawDropdown(settings);
            if (!settings.foldout) return;

            ++EditorGUI.indentLevel;

            Rect alignmentRect = EditorGUILayout.GetControlRect(false);
            EditorGUI.LabelField(alignmentRect, "Thumb awareness", EditorStyles.label);
            // Help button
            alignmentRect.x = EditorGUIUtility.singleLineHeight * 9;
            alignmentRect.width = alignmentRect.height;
            if (GUI.Button(alignmentRect, "?")) {
                EditorUtility.DisplayDialog("Thumb awareness", HelpText.thumbAwareness, "OK");
            }

            string[] thumbAwarenessOptions = {"Generate thumb-aware mode only", "Generate both thumb-aware and thumb-naive", "Generate thumb-naive only"};

            ++EditorGUI.indentLevel;
            settings.thumbAwareness = EditorGUILayout.Popup(settings.thumbAwareness, thumbAwarenessOptions);
            --EditorGUI.indentLevel;

            EditorGUILayout.LabelField("Default animations", EditorStyles.label);
            ++EditorGUI.indentLevel;
            settings.assignments.fist = EditorGUILayout.ObjectField("Fist", settings.assignments.fist, typeof(AnimationClip), true) as AnimationClip;
            settings.assignments.openHand = EditorGUILayout.ObjectField("Open Hand", settings.assignments.openHand, typeof(AnimationClip), true) as AnimationClip;
            settings.assignments.point = EditorGUILayout.ObjectField("Point", settings.assignments.point, typeof(AnimationClip), true) as AnimationClip;
            settings.assignments.peace = EditorGUILayout.ObjectField("Peace", settings.assignments.peace, typeof(AnimationClip), true) as AnimationClip;
            settings.assignments.rockNRoll = EditorGUILayout.ObjectField("Rock 'N' Roll", settings.assignments.rockNRoll, typeof(AnimationClip), true) as AnimationClip;
            settings.assignments.fingerGun = EditorGUILayout.ObjectField("Finger Gun", settings.assignments.fingerGun, typeof(AnimationClip), true) as AnimationClip;
            settings.assignments.thumbsUp = EditorGUILayout.ObjectField("Thumbs Up", settings.assignments.thumbsUp, typeof(AnimationClip), true) as AnimationClip;
            if (GUILayout.Button("Reset to defaults"))
            {
                settings.assignments = new HHSmartHands.GestureAssignments();
            }
            --EditorGUI.indentLevel;

            --EditorGUI.indentLevel;
        }

        public override void OnInspectorGUI()
        {
            var settings = target as HHSmartHands;

            // UI assignment and structure
            Rect drawRect = EditorGUILayout.GetControlRect(false, 128.0f);
            drawRect.x = (drawRect.width - 512.0f) / 2.0f;
            drawRect.width = 512.0f;

            EditorGUI.DrawPreviewTexture(drawRect, AssetCollection.banner);

            EditorGUILayout.LabelField(HHSmartHands.SMARTHANDS_VERSION, EditorStyles.miniLabel);
            EditorGUILayout.LabelField(UpdateChecker.updateRemarks, EditorStyles.miniLabel);

            // Check update button
            Rect alignmentRect = EditorGUILayout.GetControlRect(false);
            alignmentRect.x = alignmentRect.width - (alignmentRect.height * 10);
            alignmentRect.width = alignmentRect.height * 10;
            if (GUI.Button(alignmentRect, "Check for updates")) {
                UpdateChecker.SyncCheckForUpdate();
                EditorUtility.DisplayDialog("SmartHands update check", UpdateChecker.updateRemarks, "OK");
            }

            if (UpdateChecker.outOfDate) {
                EditorGUILayout.HelpBox("SmartHands is out of date. Please update as soon as is convenient to receive the latest improvements.", MessageType.Error);
            }

            EditorGUILayout.Space();

            // Check for VRCSDK2/VRCSDK3A conflict
#if (VRC_SDK_VRCSDK2 && VRC_SDK_VRCSDK3)

            EditorGUILayout.HelpBox(
                String.Join(
                    Environment.NewLine + Environment.NewLine,
                    "Uh, well, this is awkward...",
                    "You appear to have both VRCSDK2 and VRCSDK3A installed in this project.",
                    "This is never supposed to be the case, and parts of the SDK or other tools that depend on the SDK might break as a result of this conflict.",
                    "Did you upgrade this project from VRCSDK2 to VRCSDK3A?",
                    "I'm going to try and continue working regardless, but you should really consider getting this fixed, lest it develops into a more problematic issue later down the line.",
                    "If you need to ask someone for help, give them this information:",
                    "'Preprocessor definitions VRC_SDK_VRCSDK2 and VRC_SDK_VRCSDK3 detected in the same compile. The SDK may be damaged.'"
                ),
                MessageType.Error
            );

#endif

            // Check user hasn't put this onto their actual avatar object
            if (IsOnAvatar(settings.gameObject)) {
                EditorGUILayout.HelpBox(
                    String.Join(
                        Environment.NewLine + Environment.NewLine,
                        "Do not put this component onto your avatar hierarchy.",
                        "The VRCSDK will prevent you from uploading your avatar while this component is here.",
                        "Instead, create an empty object in your scene OUTSIDE of your avatar, and you will be able to specify the avatar to apply the Smart Hands to on that object."
                    ),
                    MessageType.Error
                );
                return;
            }

            settings.avatar = EditorGUILayout.ObjectField(
                "Avatar", settings.avatar, typeof(GameObject), true
            ) as GameObject;

            if (settings.avatar == null) {
                EditorGUILayout.HelpBox("There is no avatar selected.", MessageType.Error);
                return;
            }

            VRCAvatarDescriptor descriptor = settings.avatar.GetComponent<VRCAvatarDescriptor>();

            if (descriptor == null) {
                EditorGUILayout.HelpBox("This avatar lacks an avatar descriptor. You must add one before using this tool.", MessageType.Error);
                return;
            }

            if (descriptor.expressionsMenu == null) {
                EditorGUILayout.HelpBox("This avatar doesn't have a custom expression menu set.\nIf this is still the case when you Apply, one will be created for you.", MessageType.Info);
            }

            if (descriptor.expressionParameters == null) {
                EditorGUILayout.HelpBox("This avatar doesn't have a custom expression parameters object set.\nIf this is still the case when you Apply, one will be created for you.", MessageType.Info);
            }

            PlayableLayerAccessor accessor = new PlayableLayerAccessor(descriptor);

            if (!accessor.gestureLayer.isDefault && accessor.gestureLayer.animatorController != null) {
                EditorGUILayout.HelpBox("Your avatar already has an assigned Gesture layer.\nThe layer will be modified to add the Smart Hands controller layers when you press Apply.", MessageType.Warning);
            }

            // Allow a custom submenu to be targeted for the controls
            if (descriptor.expressionsMenu != null) {
                EditorGUILayout.Space();
                ++EditorGUI.indentLevel;

                EditorGUILayout.LabelField("If you want the Smart Hands control to appear in a submenu, you can specify one here.", EditorStyles.label);
                EditorGUILayout.LabelField("Otherwise, Smart Hands will be added to the top-level menu defined on your avatar.", EditorStyles.label);

                EditorGUILayout.Space();

                settings.subMenu = EditorGUILayout.ObjectField(
                    "Submenu (Optional)", settings.subMenu, typeof(VRCExpressionsMenu), true
                ) as VRCExpressionsMenu;

                if (settings.subMenu != null && !IsMenuChildOf(descriptor.expressionsMenu, settings.subMenu)) {
                    EditorGUILayout.HelpBox("The submenu specified is not a child of the menu currently set on the avatar descriptor.\nI'll still oblige and add the Smart Hands controls to this submenu, but this is probably a mistake.", MessageType.Warning);
                }

                --EditorGUI.indentLevel;
                EditorGUILayout.Space();
            } else {
                settings.subMenu = null;
            }

            DrawSettings(settings.right);
            DrawSettings(settings.left);

            EditorGUILayout.Space();

            bool parametersOK = true;

            if (descriptor.expressionParameters != null) {
                ParameterStager stager = new ParameterStager(descriptor.expressionParameters);

                if (settings.left.enabled) {
                    stager.Add("SmartHandsLeftX", VRCExpressionParameters.ValueType.Float);
                    stager.Add("SmartHandsLeftY", VRCExpressionParameters.ValueType.Float);
                    stager.Add("SmartHandsLeftMode", VRCExpressionParameters.ValueType.Int);
                }

                if (settings.right.enabled) {
                    stager.Add("SmartHandsRightX", VRCExpressionParameters.ValueType.Float);
                    stager.Add("SmartHandsRightY", VRCExpressionParameters.ValueType.Float);
                    stager.Add("SmartHandsRightMode", VRCExpressionParameters.ValueType.Int);
                }

                parametersOK = !stager.cantApply;

                List<string> parameterSummary = new List<string>();
                foreach (ParameterStager.ParameterToStage staged in stager.parameterList) {
                    parameterSummary.Add("- " + staged.name + (staged.exists ? " (already exists)" : ""));
                }

                EditorGUILayout.HelpBox(
                    String.Join(
                        Environment.NewLine,
                        $"Max parameter cost: {VRCExpressionParameters.MAX_PARAMETER_COST}",
                        $"Currently used: {stager.bitUsageBefore} ({stager.summaryBefore})",
                        $"Needed to add: {stager.bitUsageAfter - stager.bitUsageBefore} ({stager.summaryDiff})",
                        $"Total after change: {stager.bitUsageAfter} ({stager.summaryAfter})",
                        "",
                        String.Join(Environment.NewLine, parameterSummary)
                    ),
                    parametersOK ? MessageType.Info : MessageType.Error
                );
            }

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(!parametersOK);
            if (GUILayout.Button("Apply"))
            {
                Apply(settings);
            }
            EditorGUI.EndDisabledGroup();
        }

        private void Apply(HHSmartHands settings) {
            string controllerNominalGUID = Guid.NewGuid().ToString();
            string controllerName = $"HHSmartHands-{settings.avatar.name}-${controllerNominalGUID}";

            VRCAvatarDescriptor descriptor = settings.avatar.GetComponent<VRCAvatarDescriptor>();

            // Record that the descriptor will be changed by this operation so that:
            //  - The object is marked dirty, requiring the scene to be saved and thus preventing the change being lost if the user closes Unity immediately afterwards
            //  - The change to the descriptor can be undone. This won't delete the artifacts or undo the changes to the menu/parameters, but there's not much we can do about that.
            Undo.RecordObject(descriptor, $"Apply SmartHands to {settings.avatar.name}");

            PlayableLayerAccessor accessor = new PlayableLayerAccessor(descriptor);
            descriptor.customizeAnimationLayers = true;

            GeneratedController controller;
            if (!accessor.gestureLayer.isDefault && accessor.gestureLayer.animatorController != null)
                controller = new GeneratedController((AnimatorController)accessor.gestureLayer.animatorController, settings);
            else
                controller = new GeneratedController(AssetCollection.artifactFolder + "/" + controllerName + ".controller", settings);

            accessor.gestureLayer = PlayableLayerAccessor.Set(accessor.gestureLayer, controller.controller);

            descriptor.customExpressions = true;

            // If a parameters object doesn't exist, make one
            if (descriptor.expressionParameters == null) {
                VRCExpressionParameters parameters = UnityEngine.Object.Instantiate(AssetCollection.defaultParameters);
                AssetDatabase.CreateAsset(parameters, AssetCollection.artifactFolder + "/" + controllerName + "-parameters.asset");
                descriptor.expressionParameters = parameters;
            }

            // Stage and apply the parameters
            ParameterStager stager = new ParameterStager(descriptor.expressionParameters);

            if (settings.left.enabled) {
                stager.Add("SmartHandsLeftX", VRCExpressionParameters.ValueType.Float);
                stager.Add("SmartHandsLeftY", VRCExpressionParameters.ValueType.Float);
                stager.Add("SmartHandsLeftMode", VRCExpressionParameters.ValueType.Int);
            }

            if (settings.right.enabled) {
                stager.Add("SmartHandsRightX", VRCExpressionParameters.ValueType.Float);
                stager.Add("SmartHandsRightY", VRCExpressionParameters.ValueType.Float);
                stager.Add("SmartHandsRightMode", VRCExpressionParameters.ValueType.Int);
            }

            stager.Apply();

            VRCExpressionsMenu menu = descriptor.expressionsMenu;

            // If an expressions menu doesn't exist, make one
            if (descriptor.expressionsMenu == null) {
                menu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                descriptor.expressionsMenu = menu;
                AssetDatabase.CreateAsset(menu, AssetCollection.artifactFolder + "/" + controllerName + "-menu.asset");
            } else if (settings.subMenu != null) {
                menu = settings.subMenu;
            }

            // If the expressions menu already contains a submenu control called SmartHands, delete it
            string[] puppetNames = {"SmartHands (R)", "SmartHands (R/TN)", "SmartHands (L)", "SmartHands (L/TN)"};

            menu.controls.RemoveAll(control => (control.name == "SmartHands" && control.type == VRCExpressionsMenu.Control.ControlType.SubMenu) || (Array.Exists<string>(puppetNames, element => element == control.name) && control.type == VRCExpressionsMenu.Control.ControlType.TwoAxisPuppet));

            VRCExpressionsMenu.Control newControl;

            List<(HandConfigurations.Configuration, int, string, string, Texture2D)> menusToCreate = new List<(HandConfigurations.Configuration, int, string, string, Texture2D)>();

            if (settings.right.enabled && (settings.right.thumbAwareness == 0 || settings.right.thumbAwareness == 1))
                menusToCreate.Add((HandConfigurations.right, 1, "SmartHands (R)", "Right Hand", AssetCollection.menuIcon));

            if (settings.right.enabled && (settings.right.thumbAwareness == 1 || settings.right.thumbAwareness == 2))
                menusToCreate.Add((HandConfigurations.right, 2, "SmartHands (R/TN)", "Right Hand (TN)", AssetCollection.menuIconNaive));

            if (settings.left.enabled && (settings.left.thumbAwareness == 0 || settings.left.thumbAwareness == 1))
                menusToCreate.Add((HandConfigurations.left, 1, "SmartHands (L)", "Left Hand", AssetCollection.menuIcon));

            if (settings.left.enabled && (settings.left.thumbAwareness == 1 || settings.left.thumbAwareness == 2))
                menusToCreate.Add((HandConfigurations.left, 2, "SmartHands (L/TN)", "Left Hand (TN)", AssetCollection.menuIconNaive));

            // Create a submenu for SmartHands
            if (menusToCreate.Count > 1) {
                // Both hands are smart
                VRCExpressionsMenu submenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                AssetDatabase.CreateAsset(submenu, AssetCollection.artifactFolder + "/" + controllerName + "-submenu.asset");

                newControl = new VRCExpressionsMenu.Control();
                newControl.name = "SmartHands";
                newControl.type = VRCExpressionsMenu.Control.ControlType.SubMenu;
                newControl.icon = AssetCollection.menuIcon;
                newControl.subMenu = submenu;
                menu.controls.Add(newControl);

                // Generate submenu contents
                foreach (var menuToCreate in menusToCreate) {
                    var control = new VRCExpressionsMenu.Control();
                    control.name = menuToCreate.Item4;
                    control.icon = menuToCreate.Item5;
                    control.type = VRCExpressionsMenu.Control.ControlType.TwoAxisPuppet;

                    control.parameter = new VRCExpressionsMenu.Control.Parameter();
                    control.parameter.name = menuToCreate.Item1.poseModeParameter;
                    control.value = menuToCreate.Item2;

                    control.subParameters = new VRCExpressionsMenu.Control.Parameter[2];
                    control.subParameters[0] = new VRCExpressionsMenu.Control.Parameter();
                    control.subParameters[0].name = menuToCreate.Item1.poseXParameter;
                    control.subParameters[1] = new VRCExpressionsMenu.Control.Parameter();
                    control.subParameters[1].name = menuToCreate.Item1.poseYParameter;

                    submenu.controls.Add(control);
                }


                // Make sure the submenu gets saved
                EditorUtility.SetDirty(submenu);
            } else if (menusToCreate.Count == 1) {
                // Only one config, so only make a two-axis puppet
                newControl = new VRCExpressionsMenu.Control();
                newControl.name = menusToCreate[0].Item3;
                newControl.icon = menusToCreate[0].Item5;
                newControl.type = VRCExpressionsMenu.Control.ControlType.TwoAxisPuppet;

                newControl.parameter = new VRCExpressionsMenu.Control.Parameter();
                newControl.parameter.name = menusToCreate[0].Item1.poseModeParameter;
                newControl.value = menusToCreate[0].Item2;

                newControl.subParameters = new VRCExpressionsMenu.Control.Parameter[2];
                newControl.subParameters[0] = new VRCExpressionsMenu.Control.Parameter();
                newControl.subParameters[0].name = menusToCreate[0].Item1.poseXParameter;
                newControl.subParameters[1] = new VRCExpressionsMenu.Control.Parameter();
                newControl.subParameters[1].name = menusToCreate[0].Item1.poseYParameter;

                menu.controls.Add(newControl);
            }

            // Mark assets as dirty and save to disk
            EditorUtility.SetDirty(descriptor.expressionParameters);
            EditorUtility.SetDirty(menu);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
}
#else

namespace DevonsToolbox {
namespace SmartHands {
    [CustomEditor(typeof(HHSmartHands))]
    public class HHSmartHandsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var settings = target as HHSmartHands;

            // UI assignment and structure
            Rect drawRect = EditorGUILayout.GetControlRect(false, 128.0f);
            drawRect.x = (drawRect.width - 512.0f) / 2.0f;
            drawRect.width = 512.0f;

            EditorGUI.DrawPreviewTexture(drawRect, AssetCollection.banner);

            EditorGUILayout.LabelField(HHSmartHands.SMARTHANDS_VERSION, EditorStyles.miniLabel);

            EditorGUILayout.Space();

#if VRC_SDK_VRCSDK2

            EditorGUILayout.HelpBox(
                String.Join(
                    Environment.NewLine + Environment.NewLine,
                    "VRCSDK2 is present in this project instead of VRCSDK3A.",
                    "For the safety of your project, all SmartHands functionality has been disabled.",
                    "Please either remove VRCSDK2 and install VRCSDK3A, or remove the HHSmartHands folder from your project."
                ),
                MessageType.Error
            );

#else

            EditorGUILayout.HelpBox(
                String.Join(
                    Environment.NewLine + Environment.NewLine,
                    "There is no SDK in this project.",
                    "All SmartHands functionality will be disabled until VRCSDK3A is present in the project.",
                    "Please either install the SDK or remove the HHSmartHands folder from your project."
                ),
                MessageType.Error
            );

#endif
        }
    }
}
}

#endif
#endif
