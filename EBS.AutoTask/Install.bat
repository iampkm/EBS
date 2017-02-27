cd /d %~dp0
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe EBS.AutoTask.exe
Net Start EBS.AutoTask
sc config EBS.AutoTask start= auto
pause