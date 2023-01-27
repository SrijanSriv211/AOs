@ECHO OFF
TITLE Fixing corrupted system files
ECHO Starting...

sfc /scannow
DISM /Online /Cleanup-image /Restorehealth

ECHO|SET /P="Continue."
PAUSE>NUL
EXIT
