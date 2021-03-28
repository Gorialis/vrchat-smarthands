#!/bin/sh

python -V
echo "$(pwd)"

export GIT_REV_COUNT="$(git rev-list --count HEAD)"
export GIT_REV_HASH="$(git rev-parse --short HEAD)"
export GIT_REV_LONG_HASH="$(git rev-parse HEAD)"

python -m pip install -U -r DevOps/requirements.txt
python DevOps/generate_new.py
python DevOps/update_fixtures.py
python DevOps/create_assets.py
