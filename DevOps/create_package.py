# -*- coding: utf-8 -*-

import datetime
import io
import os
import pathlib
import tarfile

import yaml

import filepaths


print("Creating unitypackage..")

NOW = datetime.datetime.utcnow()

# Collect files to be included
TARGET_FILES = {}

for file in filepaths.SMARTHANDS_DIRECTORY.glob("**/*"):
    if not file.is_file():
        continue

    if file.name.endswith('.meta'):
        continue

    meta_file = file.with_name(file.name + '.meta')

    if not meta_file.exists():
        print(f"Couldn't find meta file for {file.relative_to(filepaths.ROOT_DIRECTORY)}")
        exit(1)

    with open(meta_file, 'rb') as meta_fp:
        meta_data = meta_fp.read()
        yaml_data = yaml.safe_load(meta_data)
        guid = yaml_data['guid']

    with open(file, 'rb') as file_fp:
        file_data = file_fp.read()

    pathname = file.relative_to(filepaths.ROOT_DIRECTORY).as_posix()

    TARGET_FILES[guid] = {
        "asset": file_data,
        "asset.meta": meta_data,
        "pathname": pathname.encode('utf-8'),
        "path": file  # This will be removed later. It's used for scanning needed directories.
    }

    print(f"[{guid}] {pathname}")

# Collect directories to be included
TARGET_DIRECTORIES = {}

for manifest in TARGET_FILES.values():
    path: pathlib.Path = manifest.pop('path')

    while path.parent != path and path.parent.is_relative_to(filepaths.ASSETS_DIRECTORY) and path.parent != filepaths.ASSETS_DIRECTORY:
        path = path.parent

        meta_file = path.with_name(path.name + '.meta')

        if not meta_file.exists():
            print(f"Couldn't find meta file for {path.relative_to(filepaths.ROOT_DIRECTORY)}")
            exit(1)

        with open(meta_file, 'rb') as meta_fp:
            meta_data = meta_fp.read()
            yaml_data = yaml.safe_load(meta_data)
            guid = yaml_data['guid']

            if guid in TARGET_DIRECTORIES:
                continue  # skip already checked directory

        pathname = path.relative_to(filepaths.ROOT_DIRECTORY).as_posix()

        TARGET_DIRECTORIES[guid] = {
            "asset.meta": meta_data,
            "pathname": pathname.encode('utf-8'),
        }

        print(f"<{guid}> {pathname}")


NAME_COMPONENTS = ('HHSmartHands', f'{NOW:%Y-%m-%d %H%M}', os.getenv('DISTRIBUTION_KIND', None), os.getenv('PACKAGE_TARGET', None))
PACKAGE_NAME = ' '.join(filter(None, NAME_COMPONENTS))

PACKAGE_LOCATION = filepaths.ROOT_DIRECTORY / 'UnityPackages' / f'{PACKAGE_NAME}.unitypackage'
PACKAGE_LOCATION.parent.mkdir(parents=True, exist_ok=True)

with open(PACKAGE_LOCATION, mode='wb') as package_file:
    with tarfile.open(name='archtemp.tar', fileobj=package_file, mode='w:gz') as package:
        for target in TARGET_DIRECTORIES, TARGET_FILES:
            for guid, manifest in target.items():
                for filename, data in manifest.items():
                    info = tarfile.TarInfo(name=f"{guid}/{filename}")
                    info.size = len(data)

                    package.addfile(info, io.BytesIO(data))

        with open(filepaths.RESOURCES_DIRECTORY / 'package_icon.png', 'rb') as fp:
            info = package.gettarinfo(arcname=".icon.png", fileobj=fp)
            package.addfile(info, fp)
