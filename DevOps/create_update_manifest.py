# -*- coding: utf-8 -*-

import json
import os
import sys


# Create manifest
with open(sys.argv[1]) as fp:
    json.dump({
        "latest": int(os.getenv('GIT_REV_COUNT')),
        "revisions": list(reversed(os.getenv('GIT_HISTORY').strip().split()))
    }, fp=fp, indent=4).encode('utf-8')
