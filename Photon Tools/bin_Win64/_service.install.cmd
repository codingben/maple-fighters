@echo off
REM change dir to location of script
SET mypath=%~dp0
CD %mypath%

PhotonSocketServer.exe /install %1
