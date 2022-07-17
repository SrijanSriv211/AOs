@ECHO OFF

rem echo 7z.exe
ping localhost -n 2 >nul
rem if exist 7z.exe==Echo Ok.
if not exist 7z.exe==goto BSOD1
rem echo.

rem echo 72x.exe
ping localhost -n 2 >nul
rem if exist 72x.exe==Echo Ok.
if not exist 72x.exe==goto BSOD2
rem echo.

rem echo Adapter.exe
ping localhost -n 2 >nul
rem if exist Adapter.exe==Echo Ok.
if not exist Adapter.exe==goto BSOD3
rem echo.

rem echo adm.exe
ping localhost -n 2 >nul
rem if exist adm.exe==Echo Ok.
if not exist adm.exe==goto BSODadmin
rem echo.

rem echo API.exe
ping localhost -n 2 >nul
rem if exist API.exe==Echo Ok.
if not exist API.exe==goto BSOD4
rem echo.

rem echo Bit.exe
ping localhost -n 2 >nul
rem if exist Bit.exe==Echo Ok.
if not exist Bit.exe==goto BSOD5
rem echo.

rem echo Root.exe
ping localhost -n 2 >nul
rem if exist Root.exe==Echo Ok.
if not exist Root.exe==goto BSOD6
rem echo.

rem echo Terminal.exe
ping localhost -n 2 >nul
rem if exist Terminal.exe==Echo Ok.
if not exist Terminal.exe==goto BSOD7
rem echo.

rem echo safe.exe
ping localhost -n 2 >nul
rem if exist safe.exe==Echo Ok.
if not exist safe.exe==goto BSOD9
rem echo.

ping localhost -n 3 >nul
exit

:BSOD1
echo Error 0x001
ping localhost -n 2.5 >nul
cls
call sysfail/BSOD/BSOD1.bat

:BSOD2
echo Error 0x002
ping localhost -n 2.5 >nul
cls
call sysfail/BSOD/BSOD2.bat

:BSOD3
echo Error 0x003
ping localhost -n 2.5 >nul
cls
call sysfail/BSOD/BSOD3.bat

:BSOD4
echo Error 0x004
ping localhost -n 2.5 >nul
cls
call sysfail/BSOD/BSOD4.bat

:BSOD5
echo Error 0x005
ping localhost -n 2.5 >nul
cls
call sysfail/BSOD/BSOD5.bat

:BSOD6
echo Error 0x006
ping localhost -n 2.5 >nul
cls
call sysfail/BSOD/BSOD6.bat

:BSOD7
echo Error 0x007
ping localhost -n 2.5 >nul
cls
call sysfail/BSOD/BSOD7.bat

:BSODadmin
echo Error 0x008
ping localhost -n 2.5 >nul
cls
call sysfail/BSOD/BSODadm.bat

:BSOD9
echo Error 0x009
ping localhost -n 2.5 >nul
cls
call sysfail/BSOD/BSOD9.bat