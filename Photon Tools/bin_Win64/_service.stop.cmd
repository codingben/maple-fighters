@echo off
REM change dir to location of script
SET mypath=%~dp0
CD %mypath%

SC stop "Photon Socket Server: %1"