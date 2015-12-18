@echo off

REM Navigate out of the C# Directory and into the main folder
cd..
cd..
cd..
cd..
cd..

REM Set up the structure for the system
if not exist "Data" mkdir Data
cd Data
if not exist "Refined" mkdir Refined
cd Refined
cd..
break > arp_dump.txt

REM Run an ARP Scan on the first 20 indexes of the network, and send the output to arp_dump.txt (To be filtered by substring_split)
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
