@ECHO OFF
echo ׼����װ����
rem pause ��ʱ��Ҫ
REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%
echo ��װ����...
echo ---------------------------------------------------
InstallUtil /i  D:\XDTC\MEDAS.exe
echo ---------------------------------------------------
echo ��װ����ɹ���
rem pause ��ʱ��Ҫ