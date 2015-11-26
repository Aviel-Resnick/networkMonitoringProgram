@echo off
color 0F
setlocal enableextensions enabledelayedexpansion

cd Data
cd Refined

cd..
cd..
xcopy Timestamp.bat Data\Refined
cd Data\Refined
call Timestamp.bat >> History.txt

del Timestamp.bat

break > MACS.txt

cd..

for /f "tokens=1,3" %%a in ('type data_dump.txt') do (
    if "x%%a"=="xMAC" (
	cd Refined
        echo %%b>> MACS.txt
	echo %%b>> History.txt
	cd..
    )
)

cd Refined
echo. >> History.txt

endlocal