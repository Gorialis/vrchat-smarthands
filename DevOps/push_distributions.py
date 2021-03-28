# -*- coding: utf-8 -*-

import io
import json
import os

import discord

import filepaths


webhook = discord.Webhook.from_url(os.getenv("SMARTHANDS_WEBHOOK_URL"), adapter=discord.RequestsWebhookAdapter())
git_hash = os.getenv("GIT_LONG_HASH")
packages = list((filepaths.ROOT_DIRECTORY / 'UnityPackages').glob("*.unitypackage"))
packages.sort(key=lambda p: p.parts[-1])

webhook.send(f'Builds for `{git_hash[0:7]}` https://github.com/Gorialis/vrchat-smarthands/commit/{git_hash}')

for package in packages:
    print(f"Uploading {package.parts[-1]}")

    with open(package, 'rb') as fp:
        webhook.send(file=discord.File(fp=fp, filename=package.parts[-1]))

# Create manifest
buffer = io.BytesIO(
    json.dumps({
        "latest": int(os.getenv('GIT_REV_COUNT')),
        "revisions": list(reversed(os.getenv('GIT_HISTORY').strip().split()))
    }, indent=4).encode('utf-8')
)

webhook.send(file=discord.File(fp=buffer, filename="smarthands.json"))
