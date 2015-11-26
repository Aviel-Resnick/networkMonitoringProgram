@echo off

for /F "tokens=2" %%i in ('date /t') do set mydate=%%i
set mytime=%time%
echo Timestamp: %mydate%-%mytime%
