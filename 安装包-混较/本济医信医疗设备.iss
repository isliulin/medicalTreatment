; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define MyAppName "本济医信医疗设备数据转存软件"
#define MyAppVersion "17.07.14"
#define MyAppPublisher "北京迅达天成网络科技有限公司"
#define MyAppURL "http://27776794.b2b.11467.com"
#define MyAppExeName "MEDAS.exe"
#define MyNetName "dotNet40.exe"
#define MyYinDao "GuideInstall.exe"
#define MyDeviceInit "DeviceInit.exe"
#define MyDeviceInitName "本济医信设备管理软件"

[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (生成新的GUID，点击 工具|在IDE中生成GUID。)
AppId={{A2A0372D-C6DF-4958-BF9D-66418FEB6A82}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName=D:\XDTC
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=J:\XDTC完成版170714\安装包-混较
OutputBaseFilename=本济医信医疗设备数据转存软件
SetupIconFile=J:\XDTC完成版170714\安装包-混较\image\logo.ico
Compression=lzma
SolidCompression=yes
WizardImageFile=J:\XDTC完成版170714\安装包-混较\image\logo_Lift.bmp
WizardSmallImageFile=J:\XDTC完成版170714\安装包-混较\image\logo_small.bmp
DisableDirPage=no
[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkablealone; 

[Files]
Source: "J:\XDTC完成版170714\安装包-混较\softword\MEDAS.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "J:\XDTC完成版170714\安装包-混较\softword\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

[Icons]
;Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"Name: "{commondesktop}\{#MyDeviceInitName}"; Filename: "{app}\{#MyDeviceInit}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyNetName}"; Description: "{cm:LaunchProgram,{#StringChange(MyNetName, '&', '&&')}}";   StatusMsg: "正在安装.NET4.0环境......"
Filename: "{app}\{#MyYinDao}"; Description: "{cm:LaunchProgram,{#StringChange(MyYinDao, '&', '&&')}}";   StatusMsg: "正在进行资格认证......"
Filename: "{app}\install.bat"; Description: "BAT";  StatusMsg: "正在安装软件服务......"
Filename: "{app}\Updateinstall.bat"; Description: "BAT";  StatusMsg: "正在安装更新服务......"
;Flags: skipifsilent shellexec runhidden nowait postinstall ;



