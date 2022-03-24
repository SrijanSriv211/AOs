@ECHO OFF
TITLE Fix Corrupted System Files
ECHO Fix Corrupted System Files.

sfc /scannow
DISM /Online /Cleanup-image /Restorehealth

ECHO|SET /P="Continue."
PAUSE>NUL
EXIT
