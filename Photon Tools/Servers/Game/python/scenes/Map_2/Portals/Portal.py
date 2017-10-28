from MathematicsHelper import *
from Game.InterestManagement import *
from Game.Application.SceneObjects import *

import os
import json

path = os.path.abspath(__file__)
jsonPath = os.path.splitext(path)[0] + ".json"

with open(jsonPath) as data_file:
    data = json.load(data_file)

x = data["Position"]["x"]
y = data["Position"]["y"]

position = Vector2(x, y)
playerPosition = Vector2(18, -1.75)
map = 2

portalSceneObject = Portal(position, playerPosition, map)

scene.AddSceneObject(portalSceneObject)