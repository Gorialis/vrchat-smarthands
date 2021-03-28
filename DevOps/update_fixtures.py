# -*- coding: utf-8 -*-

import datetime
import os
import pathlib
import re

from jinja2 import Environment
from jinja2.environment import Template
from jinja2.loaders import BaseLoader

import filepaths


ENVIRONMENT = Environment(loader=BaseLoader())


def filter(function):
    ENVIRONMENT.filters[function.__name__] = function
    ENVIRONMENT.globals[function.__name__ + '_filter'] = function

    return function


@filter
def csharp(value):
    if value is None:
        return 'null'
    elif isinstance(value, bool):
        return 'true' if value else 'false'
    elif isinstance(value, int):
        return str(value)
    elif isinstance(value, float):
        return f"{value}f"
    elif isinstance(value, str):
        return '"' + value.replace('"', '\\"') + '"'
    elif isinstance(value, datetime.datetime):
        return f'new DateTime({value.year}, {value.month}, {value.day}, {value.hour}, {value.minute}, {value.second})'

@filter
def guid(path):
    if isinstance(path, str):
        path = pathlib.Path(path)

    with open(path.with_name(path.name + '.meta'), mode='r', encoding='utf-8') as meta_file:
        match = re.search(r"guid: ([0-9a-f]{32})", meta_file.read())

        if not match:
            raise ValueError(f"Couldn't find guid for {path}")

        return match.group(1)


# Globals for fixtures
ENVIRONMENT.globals.update({
    'ASSETS': filepaths.ASSETS_DIRECTORY,
    'BUILD_DATE': datetime.datetime.utcnow(),
    'COMPILED': os.getenv('DISTRIBUTION_KIND', None) == 'DLL',
    'GIT_HASH': os.getenv("GIT_REV_LONG_HASH", 'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX'),
    'GIT_REVISION': int(os.getenv("GIT_REV_COUNT", 0)),
    'PACKAGE_TARGET': os.getenv("PACKAGE_TARGET", None),
    'ROOT': filepaths.ROOT_DIRECTORY,
    'SMARTHANDS': filepaths.SMARTHANDS_DIRECTORY,
})


# Loop
for fixture_file in filepaths.SMARTHANDS_DIRECTORY.glob("**/*.jinja"):
    output_file = fixture_file.with_name(fixture_file.name[:-6])

    print(f"{fixture_file.relative_to(filepaths.ROOT_DIRECTORY)} => {output_file.relative_to(filepaths.ROOT_DIRECTORY)}")

    with open(fixture_file, mode='r', encoding='utf-8') as input_file:
        template: Template = ENVIRONMENT.from_string(input_file.read())

        with open(output_file, mode='w', encoding='utf-8') as output_file:
            output_data = template.render()

            # Jinja likes to remove my trailing newlines
            if not output_data.endswith('\n'):
                output_data += '\n'

            output_file.write(output_data)
