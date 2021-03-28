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
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace DevonsToolbox {
namespace SmartHands {
    /// <summary>
    /// ParameterStager is the encapsulation of a parameter 'transaction'.
    /// That is, it takes an existing VRCExpressionParameters, and takes parameters that should be added to it.
    /// It then determines which parameters can be recycled, which parameters need to be added, and thus overall whether the transaction is possible or not.
    /// </summary>
    public class ParameterStager {
        public class ParameterToStage {
            public ParameterToStage(string name, VRCExpressionParameters.ValueType type, bool exists) {
                this.name = name;
                this.type = type;
                this.exists = exists;
            }

            public string name;
            public VRCExpressionParameters.ValueType type;
            public bool exists;
        }

        public ParameterStager(VRCExpressionParameters parameters) {
            this.parameters = parameters;

            foreach (var param in parameters.parameters) {
                if (param.name != "") {
                    bitUsageBefore += VRCExpressionParameters.TypeCost(param.valueType);
                    usageTypesBefore[param.valueType] = (usageTypesBefore.ContainsKey(param.valueType) ? usageTypesBefore[param.valueType] : 0) + 1;
                }
            }

            bitUsageAfter = bitUsageBefore;
            usageTypesAfter = new Dictionary<VRCExpressionParameters.ValueType, int>(usageTypesBefore);
        }

        public ParameterToStage Add(string name, VRCExpressionParameters.ValueType type) {
            foreach (var param in parameters.parameters) {
                if (param.name == name) {
                    // Adjust type cost
                    if (type != param.valueType) {
                        bitUsageAfter += VRCExpressionParameters.TypeCost(type) - VRCExpressionParameters.TypeCost(param.valueType);
                        usageTypesAfter[param.valueType] -= 1;
                        usageTypesAfter[type] = (usageTypesAfter.ContainsKey(type) ? usageTypesAfter[type] : 0) + 1;

                        usageTypesRemoved[param.valueType] = (usageTypesRemoved.ContainsKey(param.valueType) ? usageTypesRemoved[param.valueType] : 0) + 1;
                        usageTypesAdded[type] = (usageTypesAdded.ContainsKey(type) ? usageTypesAdded[type] : 0) + 1;
                    }

                    ParameterToStage stagedMatch = new ParameterToStage(name, type, true);
                    parameterList.Add(stagedMatch);
                    return stagedMatch;
                }
            }

            bitUsageAfter += VRCExpressionParameters.TypeCost(type);
            usageTypesAfter[type] = (usageTypesAfter.ContainsKey(type) ? usageTypesAfter[type] : 0) + 1;
            usageTypesAdded[type] = (usageTypesAdded.ContainsKey(type) ? usageTypesAdded[type] : 0) + 1;

            ParameterToStage staged = new ParameterToStage(name, type, false);
            parameterList.Add(staged);
            return staged;
        }

        /// <summary>
        /// Apply the parameter transaction.
        /// This does NOT care about whether the transaction is actually fully completable or not, i.e. it will attempt to make all changes regardless of <value>cantApply</value>.
        /// <remarks>If <value>cantApply</value> is true, this transaction will result in bad expression params, so please check this value before calling this function.</remarks>
        /// </summary>
        public void Apply() {
            // First delete empty slots
            var paramCollection = parameters.parameters.Where(x => x.name != "").ToList();

            foreach (ParameterToStage staged in parameterList) {
                try {
                    paramCollection.First(x => x.name == staged.name).valueType = staged.type;
                } catch {
                    var param = new VRCExpressionParameters.Parameter();
                    param.name = staged.name;
                    param.valueType = staged.type;

                    paramCollection.Add(param);
                }
            }

            parameters.parameters = paramCollection.ToArray();
        }

        public VRCExpressionParameters parameters;

        public int bitUsageBefore = 0;
        public Dictionary<VRCExpressionParameters.ValueType, int> usageTypesBefore = new Dictionary<VRCExpressionParameters.ValueType, int>();

        public int bitUsageAfter = 0;
        public Dictionary<VRCExpressionParameters.ValueType, int> usageTypesAfter = new Dictionary<VRCExpressionParameters.ValueType, int>();

        public Dictionary<VRCExpressionParameters.ValueType, int> usageTypesRemoved = new Dictionary<VRCExpressionParameters.ValueType, int>();
        public Dictionary<VRCExpressionParameters.ValueType, int> usageTypesAdded = new Dictionary<VRCExpressionParameters.ValueType, int>();

        public string summaryBefore { get {
            return String.Join(", ", usageTypesBefore.Select(x => String.Format("{1} {0}", x.Key, x.Value)));
        }}

        public string summaryAfter { get {
            return String.Join(", ", usageTypesAfter.Select(x => String.Format("{1} {0}", x.Key, x.Value)));
        }}

        public string summaryDiff { get {
            return String.Join(", ",
                Enumerable.Concat(
                    usageTypesRemoved.Select(x => String.Format("-{1} {0}", x.Key, x.Value)),
                    usageTypesAdded.Select(x => String.Format("+{1} {0}", x.Key, x.Value))
                )
            );
        }}

        public bool cantApply { get {
            return bitUsageAfter > VRCExpressionParameters.MAX_PARAMETER_COST;
        }}

        public List<ParameterToStage> parameterList = new List<ParameterToStage>();
    }
}
}

#endif
