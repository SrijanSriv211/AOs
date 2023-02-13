@ECHO OFF
title Starting build

for %%i in (.) do set current_folder=%%~nxi
if %current_folder%=="scripts" cd..

set filename="build.txt"

rem Activate venv before compiling.
rem This is make sure nothing breaks while compiling.
call .venv\Scripts\activate

rem Check if AOs folder exists, if yes, remove it.
if exist AOs rmdir /s /q AOs
if not exist %filename% echo. > %filename%

rem Compile project
dotnet publish -c Release -o ./AOs

echo. && echo Compiling C++ scripts
rem g++ "Sysfail\rp\safe.cpp" -o Sysfail/rp/safe.exe

echo. && echo Compiling Python scripts
rem pyinstaller --onefile --icon=img/UPR.ico "Sysfail\RECOVERY\SoftwareDistribution\UpdatePackages\UPR.py" --distpath Sysfail\RECOVERY\SoftwareDistribution\UpdatePackages

rem Move all necessary folders to the build folder.
echo. && echo Finishing build
robocopy "Sysfail" "AOs/Sysfail" /e /nfl /ndl /njh /njs /nc /ns /np /is /it /xf *.py
robocopy "Sysfail/RECOVERY" "AOs" /e /nfl /ndl /njh /njs /nc /ns /np /is /it /xf *.py

rem Delete all unnecessary files and folders.
rmdir /s /q bin,obj,build
del *.spec

rem Update build number.
rem Read each line of the input file, add 1 to the number on that line, and write the result to the output file
for /f "usebackq delims=" %%a in (%filename%) do (
    set /a number=%%a+1
    echo %number% > %filename%
)

echo Done.
