# <img src=".github/assets/smarthands-profile-editor.svg" height="32"> SmartHands autoupdater branch

This branch is an info branch for SmartHands' auto-updating feature. If you are interested in SmartHands itself, you should go to the [main](https://github.com/Gorialis/vrchat-smarthands/tree/main) branch.

When a new release for SmartHands is made, GitHub Actions will create a manifest file covering the commit history and commit it here.

Existing versions of SmartHands check against this file either when the editor starts up, when SmartHands is imported for the first time, or when the user explicitly presses to check for an update.

If SmartHands discovers it is out of date, it will warn the user, asking them to update (because updating is important, and not enough people do it!).

There's not really anything else to say on this branch. The workflow itself is on [main](https://github.com/Gorialis/vrchat-smarthands/tree/main) and triggers on tag publish (which is also the trigger for creating a release).
