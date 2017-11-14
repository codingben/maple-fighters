from MathematicsHelper import *

sceneId = 2
sceneSize = Vector2(40, 5)
regionSize = Vector2(10, 5)

sceneContainer.AddScene(sceneId, sceneSize, regionSize)

lowerBound = Vector2(-100, -100)
upperBound = Vector2(100, 100)
gravity = Vector2(0, -9.8)
doSleep = True
drawPhysics = True

sceneContainer.AddPhysics(sceneId, lowerBound, upperBound, gravity, doSleep, drawPhysics)