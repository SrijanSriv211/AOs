@ECHO OFF
title Starting build

@REM Check if AOs folder exists, if yes, remove it.
if EXIST AOs rmdir /s /q AOs 

@REM Compile project
dotnet publish -c Release -o ./AOs

echo. && echo Compiling safe.cpp
g++ safe.cpp -o Sysfail/rp/safe.exe

echo. && echo Compiling UPR.py
pyinstaller -y --clean --onefile --icon=img/UPR.ico --add-data "img;img" -n UPR UPR.py --distpath SoftwareDistribution\UpdatePackages\

@REM Move all necessary folders to the build folder.
robocopy "Sysfail" "AOs/Sysfail" /e /nfl /ndl /njh /njs /nc /ns /np /is /it
robocopy "Sysfail/RECOVERY" "AOs" /e /nfl /ndl /njh /njs /nc /ns /np /is /it
rmdir /s /q bin,obj,build
del UPR.spec
