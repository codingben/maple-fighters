from MathematicsHelper import *
from InterestManagement import *
from Game.Application.GameObjects import *
from Game.Common import Maps

import os
import json

path = os.path.abspath(__file__)
jsonPath = os.path.splitext(path)[0] + ".json"

with open(jsonPath) as data_file:
    data = json.load(data_file)

x = data["Position"]["x"]
y = data["Position"]["y"]

position = Vector2(x, y)
destinationMap = Maps.Map_2

portalSceneObject = Portal(position, destinationMap)

scene.CreateSceneObject(portalSceneObject)