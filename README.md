# Homefield Sentinel

The Homefield Sentinel is a Windows Honeypot Service that watches for Audit events in the event log. 

In particular it looks for access to a file named passwords.txt. When the file is opened, the Sentinel will write the details of the audit event to the sentinel.log file and additionally also send an email to notify that someone accessed the file.

The passwords.txt file is hardcoded for now, but can easily be updated, or ideally be modified to read the files to monitor for from the Homefield.Sentinel.exe.config file.


## Auditing Setup
-) Configure Windows to log audit events (update local group policy)
-) Create the honeypot file called passwords.txt on the machine and add a SACL enty to emit audit events on access/read.


## Installation

installutil Homefield.Sentinel.exe

During install a couple of configuration settings will be needed (here is what it will look like):

Smtp Server (Default smtp-mail.outlook.com):
Smtp Server Port (Default 587):
Email Account for Notifications: 
Password:


## Launch the Service

net start "Homefield Sentinel"

There is a log file in the directory of the Homefield.Sentine.exe named sentinel.log


## Uninstall:

installutil /u Homefield.Sentinel.exe


## License

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
