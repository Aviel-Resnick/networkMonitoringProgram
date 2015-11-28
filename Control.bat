@echo off

if not exist "Data" mkdir Data
cd Data
break > Allowed_Users.txt
if not exist "Refined" mkdir Refined
cd Refined
cd..
break > arp_dump.txt

arp -a 192.168.1.1 >> arp_dump.txt
arp -a 192.168.1.2 >> arp_dump.txt
arp -a 192.168.1.3 >> arp_dump.txt
arp -a 192.168.1.4 >> arp_dump.txt
arp -a 192.168.1.5 >> arp_dump.txt
arp -a 192.168.1.6 >> arp_dump.txt
arp -a 192.168.1.7 >> arp_dump.txt
arp -a 192.168.1.8 >> arp_dump.txt
arp -a 192.168.1.9 >> arp_dump.txt
arp -a 192.168.1.10 >> arp_dump.txt
arp -a 192.168.1.11 >> arp_dump.txt
arp -a 192.168.1.12 >> arp_dump.txt
arp -a 192.168.1.13 >> arp_dump.txt
arp -a 192.168.1.14 >> arp_dump.txt
arp -a 192.168.1.15 >> arp_dump.txt
arp -a 192.168.1.16 >> arp_dump.txt
arp -a 192.168.1.17 >> arp_dump.txt
arp -a 192.168.1.18 >> arp_dump.txt
arp -a 192.168.1.19 >> arp_dump.txt
arp -a 192.168.1.20 >> arp_dump.txt

cd..

call refinary.bat

