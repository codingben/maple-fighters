@echo off

cd ..
for %%* in (.) do set CurrDirName=%%~nx*
echo %CurrDirName%

cd ../../bin_Win64
_service.uninstall.cmd %CurrDirName%