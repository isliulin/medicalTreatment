; �ű��� Inno Setup �ű��� ���ɣ�
; �йش��� Inno Setup �ű��ļ�����ϸ��������İ����ĵ���

#define MyAppName "����ҽ��ҽ���豸����ת�����"
#define MyAppVersion "17.07.14"
#define MyAppPublisher "����Ѹ���������Ƽ����޹�˾"
#define MyAppURL "http://27776794.b2b.11467.com"
#define MyAppExeName "MEDAS.exe"
#define MyNetName "dotNet40.exe"
#define MyYinDao "GuideInstall.exe"
#define MyDeviceInit "DeviceInit.exe"
#define MyDeviceInitName "����ҽ���豸�������"

[Setup]
; ע: AppId��ֵΪ������ʶ��Ӧ�ó���
; ��ҪΪ������װ����ʹ����ͬ��AppIdֵ��
; (�����µ�GUID����� ����|��IDE������GUID��)
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
OutputDir=J:\XDTC��ɰ�170714\��װ��-���
OutputBaseFilename=����ҽ��ҽ���豸����ת�����
SetupIconFile=J:\XDTC��ɰ�170714\��װ��-���\image\logo.ico
Compression=lzma
SolidCompression=yes
WizardImageFile=J:\XDTC��ɰ�170714\��װ��-���\image\logo_Lift.bmp
WizardSmallImageFile=J:\XDTC��ɰ�170714\��װ��-���\image\logo_small.bmp
DisableDirPage=no
[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkablealone; 

[Files]
Source: "J:\XDTC��ɰ�170714\��װ��-���\softword\MEDAS.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "J:\XDTC��ɰ�170714\��װ��-���\softword\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; ע��: ��Ҫ���κι���ϵͳ�ļ���ʹ�á�Flags: ignoreversion��

[Icons]
;Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"Name: "{commondesktop}\{#MyDeviceInitName}"; Filename: "{app}\{#MyDeviceInit}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyNetName}"; Description: "{cm:LaunchProgram,{#StringChange(MyNetName, '&', '&&')}}";   StatusMsg: "���ڰ�װ.NET4.0����......"
Filename: "{app}\{#MyYinDao}"; Description: "{cm:LaunchProgram,{#StringChange(MyYinDao, '&', '&&')}}";   StatusMsg: "���ڽ����ʸ���֤......"
Filename: "{app}\install.bat"; Description: "BAT";  StatusMsg: "���ڰ�װ�������......"
Filename: "{app}\Updateinstall.bat"; Description: "BAT";  StatusMsg: "���ڰ�װ���·���......"
;Flags: skipifsilent shellexec runhidden nowait postinstall ;



