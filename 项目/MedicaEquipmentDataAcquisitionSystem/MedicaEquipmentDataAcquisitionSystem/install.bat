@ECHO OFF
echo 准备安装服务
rem pause 暂时不要
REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%
echo 安装服务...
echo ---------------------------------------------------
InstallUtil /i  D:\XDTC\MEDAS.exe
echo ---------------------------------------------------
echo 安装服务成功！
rem pause 暂时不要