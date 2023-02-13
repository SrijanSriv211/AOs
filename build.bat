@ECHO OFF
title Starting build

@REM Activate venv before compiling.
@REM This is make sure nothing breaks while compiling.
call .venv\Scripts\activate

@REM Check if AOs folder exists, if yes, remove it.
if EXIST AOs rmdir /s /q AOs

@REM Compile project
dotnet publish -c Release -o ./AOs

echo. && echo Compiling C++ scripts
g++ "Sysfail\rp\safe.cpp" -o Sysfail/rp/safe.exe

echo. && echo Compiling Python scripts
pyinstaller --onefile --icon=img/UPR.ico "Sysfail\RECOVERY\SoftwareDistribution\UpdatePackages\UPR.py" --distpath Sysfail\RECOVERY\SoftwareDistribution\UpdatePackages

@REM Move all necessary folders to the build folder.
echo. && echo Finishing build
robocopy "Sysfail" "AOs/Sysfail" /e /nfl /ndl /njh /njs /nc /ns /np /is /it /xf *.py
robocopy "Sysfail/RECOVERY" "AOs" /e /nfl /ndl /njh /njs /nc /ns /np /is /it /xf *.py
rmdir /s /q bin,obj,build
del *.spec
