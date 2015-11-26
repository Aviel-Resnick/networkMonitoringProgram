@echo off

if not exist "Data" mkdir Data
cd Data
break > Allowed_Users.txt
if not exist "Refined" mkdir Refined
cd Refined
cd..

nmap 192.168.1.1-20 > data_dump.txt

cd..

call refinary.bat

