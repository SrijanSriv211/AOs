@ECHO OFF
title Starting build

rem Activate venv before compiling.
rem This is make sure nothing breaks while compiling.
call .venv\Scripts\activate

rem Check if AOs folder exists, if yes, remove it.
if exist AOs rmdir /s /q AOs

rem Compile project
dotnet publish -c Release -o ./AOs

echo. && echo Compiling C++ scripts
g++ "Sysfail\rp\safe.cpp" -o Sysfail/rp/safe.exe

echo. && echo Compiling Python scripts
pyinstaller --onefile --icon=img/UPR.ico "Sysfail\RECOVERY\SoftwareDistribution\UpdatePackages\UPR.py" --distpath Sysfail\RECOVERY\SoftwareDistribution\UpdatePackages
pyinstaller --onefile --icon=img/exe.ico "Sysfail\RECOVERY\Files.x72\root\ext\wiki.py" --distpath Sysfail\RECOVERY\Files.x72\root\ext
pyinstaller --onefile --icon=img/exe.ico "Sysfail\RECOVERY\Files.x72\root\ext\ply.py" --distpath Sysfail\RECOVERY\Files.x72\root\ext
pyinstaller --onefile --icon=img/exe.ico "Sysfail\RECOVERY\Files.x72\root\ext\srh.py" --distpath Sysfail\RECOVERY\Files.x72\root\ext

rem Move all necessary folders to the build folder.
echo. && echo Finishing build
robocopy "Sysfail" "AOs/Sysfail" /e /nfl /ndl /njh /njs /nc /ns /np /is /it /xf *.py
robocopy "Sysfail/RECOVERY" "AOs" /e /nfl /ndl /njh /njs /nc /ns /np /is /it /xf *.py

rem Update the build no.
echo. && echo Updating build no.
python scripts\update_build_no.py

rem Delete all unnecessary files and folders.
rmdir /s /q bin,obj,build
del *.spec
echo Done.
