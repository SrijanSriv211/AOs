@ECHO OFF

if not exist Temp.txt==Echo Temporary files were deleted already.
if exist Temp.txt==del Temp.txt Echo Temporary files are deleted.
rem set /p = Done!
exit
