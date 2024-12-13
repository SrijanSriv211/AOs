@echo off

if "%cd%\" == "%~dp0" cd ..
python -B scripts/build.py %*
