from MathematicsHelper import *
from InterestManagement import *
from Game.Application.GameObjects import *

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

message = "Dangerously there!!"

if direction > 0:
	guardian = Guardian(name, position, Direction.Left)
	scene.CreateSceneObject(guardian)
	guardian.CreateBubbleMessageNotifier(message)
else:
	guardian = Guardian(name, position, Direction.Right)
	scene.CreateSceneObject(guardian)
	guardian.CreateBubbleMessageNotifier(message)