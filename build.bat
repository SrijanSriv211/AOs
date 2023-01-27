@ECHO OFF
title Starting build

@REM Check if AOs folder exists, if yes, remove it.
if EXIST AOs rmdir /s /q AOs 

@REM First compile AOs with Dotnet then safe with gcc (C++)
dotnet publish -c Release -o ./AOs

echo Compiling safe.cpp
g++ safe.cpp -o Sysfail/rp/safe.exe

@REM Move all necessary folders to the build folder.
robocopy "Sysfail" "AOs/Sysfail" /e /nfl /ndl /njh /njs /nc /ns /np /is /it
robocopy "Sysfail/RECOVERY" "AOs" /e /nfl /ndl /njh /njs /nc /ns /np /is /it
rmdir /s /q bin,obj
