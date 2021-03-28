# -*- coding: utf-8 -*-

import filepaths

ARTIFACTS_DIR = filepaths.SMARTHANDS_DIRECTORY / 'Artifacts'

for file in ARTIFACTS_DIR.glob("*"):
    if file.parts[-1] in ('stub.stub', 'stub.stub.meta'):
        continue

    print("Removing", file.relative_to(filepaths.ROOT_DIRECTORY).as_posix())

    file.unlink()
