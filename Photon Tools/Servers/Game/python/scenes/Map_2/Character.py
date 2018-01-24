from MathematicsHelper import *
from Game.InterestManagement import *

import os
import json

path = os.path.abspath(__file__)
jsonPath = os.path.splitext(path)[0] + ".json"

with open(jsonPath) as data_file:
    data = json.load(data_file)

x = data["Position"]["x"]
y = data["Position"]["y"]
direction = data["Direction"]

position = Vector2(x, y)
size = Vector2(0.3625, 0.825)

if direction > 0:
	transformDetails = TransformDetails(position, size, Direction.Left)
	scene.AddCharacterSpawnDetails(transformDetails)
else:
	transformDetails = TransformDetails(position, size, Direction.Right)
	scene.AddCharacterSpawnDetails(transformDetails)