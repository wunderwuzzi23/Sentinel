# Homefield Sentinel

The Homefield Sentinel is a basic Windows Honeypot Service that watches for Audit events in the event log. In particular it looks for access to a file named passwords.txt. 
When the file is accessed, it will write the infromation to a log file and also send email to notify that someone accessed the file.

## Pre-Condition:
## ==============
-) Configure Windows to log audit events
-) Create a file called passwords.txt on the machine and add a SACL enty to emit audit events on access/read.


## Installation:
## =============

installutil Homefield.Sentinel.exe

During install a couple of configuration settings will be needed (here is what it will look like)

**********************************************
*** Homefield Sentinel Setup Configuration ***
**********************************************
Smtp Server (Default smtp-mail.outlook.com):
Smtp Server Port (Default 587):
Email Account for Notifications: 
Password:

**********************************************
*** Install and Configuration Complete     ***
*** Run: net start "Homefield Sentinel"    ***
**********************************************

The Commit phase completed successfully.

The transacted install has completed.

## Launch the Service
## ==================

 net start "Homefield Sentinel"

 ## Launch the Service
 ## ==================
 There is a log file, named sentinel.log (in \windows\system32\sentinel.log).


## Uninstall:
## ==========
installutil /u Homefield.Sentinel.exe


## License
## =======

MIT License

Copyright (c) 2019 WUNDERWUZZI, LLC (Johann Rehberger)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.