# -*- coding: utf-8 -*-

import colorsys
import os

from PIL import Image, ImageDraw

import filepaths


LONG_HASH = os.getenv('GIT_REV_LONG_HASH', '').strip()


with Image.open(filepaths.RESOURCES_DIRECTORY / 'smart-hands-banner.png') as banner:
    draw = ImageDraw.Draw(banner)

    for index, character in enumerate(LONG_HASH):
        r, g, b = colorsys.hsv_to_rgb(int(character, 16) / 16, 1, 1)
        r, g, b = [int(x * 255) for x in (r, g, b)]

        draw.rectangle(
            (0, (8 * index), 8, (8 * (index + 1))),
            fill=(r, g, b)
        )

    with open(filepaths.SMARTHANDS_DIRECTORY / 'Textures' / 'smart-hands-banner.png', mode='wb') as output_fp:
        banner.save(output_fp, format='png')
