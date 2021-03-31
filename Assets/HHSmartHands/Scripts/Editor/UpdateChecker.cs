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
using UnityEngine.Networking;


namespace DevonsToolbox {
namespace SmartHands {
    /// <summary>
    /// UpdateChecker checks for updates.
    /// </summary>
    public static class UpdateChecker {
        public static int? latestRevision = null;
        public static string latestHash = null;

        public static string currentHash = null;

        [System.Serializable]
        private class UpdateManifest {
            public int latest;
            public string[] revisions;
        }

        [InitializeOnLoadMethod]
        private static void Initialize() {
            SyncCheckForUpdate();
        }

        public static void SyncCheckForUpdate() {
            latestRevision = null;
            latestHash = currentHash = null;

            var request = UnityWebRequest.Get("https://raw.githubusercontent.com/Gorialis/vrchat-smarthands/info/auto-updater/manifest.json");

            request.SendWebRequest();

            // Poll synchronously
            while (!request.isDone && !request.isHttpError && !request.isNetworkError) {}

            if (request.isHttpError || request.isNetworkError || !string.IsNullOrWhiteSpace(request.error)) {
                return;
            }

            UpdateManifest manifest = JsonUtility.FromJson<UpdateManifest>(request.downloadHandler.text);

            latestRevision = manifest.latest;
            latestHash = manifest.revisions[manifest.latest - 1];

            if (HHSmartHandsVersion.gitRevision <= manifest.latest)
                currentHash = manifest.revisions[HHSmartHandsVersion.gitRevision - 1];

            Debug.Log($"SmartHands retrieved version manifest: rev{latestRevision}#{latestHash.Substring(0, 7)}; we are rev{HHSmartHandsVersion.gitRevision}#{HHSmartHandsVersion.gitHash.Substring(0, 7)} -- {updateRemarks}");
        }

        public static bool outOfDate {
            get {
                if (latestRevision is null)
                    return false;

                if (HHSmartHandsVersion.gitRevision < latestRevision)
                    return true;

                if (HHSmartHandsVersion.gitRevision == latestRevision && HHSmartHandsVersion.gitHash != latestHash)
                    return true;

                return false;
            }
        }

        public static string updateRemarks {
            get {
                if (latestRevision is null)
                    return "Attempt to check for update failed (are you not connected to the internet?)";

                if (HHSmartHandsVersion.gitRevision < latestRevision)
                    return "SmartHands is out of date (old revision), please update!";

                if (HHSmartHandsVersion.gitRevision == latestRevision && HHSmartHandsVersion.gitHash != latestHash)
                    return "SmartHands is out of date (hash mismatch), please fix or update!";

                if (HHSmartHandsVersion.gitRevision > latestRevision)
                    return $"SmartHands version is ahead {HHSmartHandsVersion.gitRevision - latestRevision} revisions (is this a development build?)";

                return "SmartHands is up to date. :3";
            }
        }
    }
}
}

#endif
