@ECHO OFF
title Starting build

rem Activate venv before compiling.
rem This is make sure nothing breaks while compiling.
call .venv\Scripts\activate

rem Check if AOs folder exists, if yes, remove it.
if exist AOs rmdir /s /q AOs

rem Compile project
dotnet publish -c Release -o ./AOs

@REM echo. && echo Compiling scripts
@REM g++ "Sysfail/rp/safe.cpp" -o Sysfail/rp/safe.exe
@REM python scripts/cxthon.py --spec scripts/setup.spec
@REM python scripts/update_build_no.py

rem Move all necessary folders to the build folder.
echo. && echo Finishing build
robocopy "Sysfail" "AOs/Sysfail" /e /nfl /ndl /njh /njs /nc /ns /np /is /it /xf *.py
robocopy "Sysfail/RECOVERY" "AOs" /e /nfl /ndl /njh /njs /nc /ns /np /is /it /xf *.py

rem Delete all unnecessary files and folders.
rmdir /s /q bin,obj,build
del *.spec
echo Done.
