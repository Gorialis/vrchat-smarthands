# -*- coding: utf-8 -*-

import os

git_revision_count = os.getenv("GIT_REV_COUNT")
git_revision_hash = os.getenv("GIT_REV_HASH")

print(f"{git_revision_count=} {git_revision_hash=}")
