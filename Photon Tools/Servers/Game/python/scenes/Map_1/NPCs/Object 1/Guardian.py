from MathematicsHelper import *
from Game.InterestManagement import *

import os
import json

path = os.path.abspath(__file__)
jsonPath = os.path.splitext(path)[0] + ".json"

with open(jsonPath) as data_file:
    data = json.load(data_file)

name = data["Name"]
x = data["Position"]["x"]
y = data["Position"]["y"]
direction = data["Direction"]

position = Vector2(x, y)

if direction > 0:
	guardian = SceneObject(name, position, Direction.Left)
	scene.CreateSceneObject(guardian)
else:
	guardian = SceneObject(name, position, Direction.Right)
	scene.CreateSceneObject(guardian)