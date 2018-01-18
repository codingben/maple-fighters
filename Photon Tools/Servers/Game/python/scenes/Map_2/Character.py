from MathematicsHelper import *
from Shared.Game.Common import Directions

import os
import json

path = os.path.abspath(__file__)
jsonPath = os.path.splitext(path)[0] + ".json"

with open(jsonPath) as data_file:
    data = json.load(data_file)

x = data["Position"]["x"]
y = data["Position"]["y"]

position = Vector2(x, y)

direction = data["Direction"]

if direction > 0:
	scene.AddCharacterSpawnPosition(position, Directions.Left)
else:
	scene.AddCharacterSpawnPosition(position, Directions.Right)