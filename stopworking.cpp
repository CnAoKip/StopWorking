//-lwer -finput-charset=GBK -fexec-charset=GBK
#define RANDOMPOP 
#define _WIN32_WINNT 0x602
#define HREPORT void*
#define PWER_SUBMIT_RESULT int*
#define WER_MAX_PREFERRED_MODULES_BUFFER 10
#include <windows.h>
#include <shellapi.h>
#include <werapi.h> 
#include <winable.h>

LPCWSTR lpPaths[13] = {
	L"explorer.exe",
	L"taskmgr.exe",
	L"winlogon.exe",
	L"csrss.exe",
	L"dwm.exe",
	L"cmd.exe",
	L"svchost.exe",
	L"conhost.exe",
	L"smss.exe",
	L"wininit.exe",
	L"werfault.exe",
	L"winver.exe",
	L"regedit.exe" 
	
};

HCRYPTPROV prov;

int random() {
	if (prov == NULL)
		if (!CryptAcquireContext(&prov, NULL, NULL, PROV_RSA_FULL, CRYPT_SILENT | CRYPT_VERIFYCONTEXT))
			ExitProcess(1);

	int out;
	CryptGenRandom(prov, sizeof(out), (BYTE *)(&out));
	return out & 0x7fffffff;
}


int WINAPI ReportError(LPCWSTR lpPath, LPCWSTR lpCloseText, LPCWSTR lpDescription){
	HREPORT hReport;
	WER_REPORT_INFORMATION wri;
	ZeroMemory(&wri, sizeof(wri));
	wri.dwSize = sizeof(wri);
	wri.hProcess = GetCurrentProcess();
	lstrcpyW(wri.wzApplicationPath, lpPath);
	int wsr;
	WerReportCreate(L"123", WerReportApplicationCrash, &wri, &hReport);
	
	WerReportSetUIOption(hReport, WerUIIconFilePath, lpPath);
	if(lpCloseText) WerReportSetUIOption(hReport, WerUICloseText, lpCloseText);

	if(lpDescription){
		WCHAR DlgHeader[565];
		wsprintfW(DlgHeader, L"%s 已停止工作", lpDescription);
		WerReportSetUIOption(hReport, WerUIIconFilePath, lpPath);
		WerReportSetUIOption(hReport, WerUIConsentDlgHeader, DlgHeader);
	}
	
	WerReportSubmit(hReport, WerConsentAlwaysPrompt, 1024|8, &wsr);
	WerReportCloseHandle(hReport);
	return GetLastError();
}



int WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd){
	//BlockInput(TRUE);
	DWORD tid;
	int w = GetSystemMetrics(SM_CXSCREEN), h = GetSystemMetrics(SM_CYSCREEN);
	int cx, cy;
	
#ifdef DRAGACCEPT
	int argc;
	LPWSTR *argv = CommandLineToArgvW(GetCommandLineW(), &argc);
	if(argc >= 4)ReportError(argv[1], argv[2], argv[3]);
	else if(argc>=3)ReportError(argv[1], argv[2], NULL);
	else if(argc>=2)ReportError(argv[1], NULL, NULL);
	Sleep(50);
	HWND hwnd = GetForegroundWindow();
	RECT rc;
	if(hwnd) {
		GetWindowRect(hwnd, &rc);
		cx = rc.right - rc.left, cy = rc.bottom - rc.top;
		SetWindowPos(hwnd, HWND_TOPMOST, random()%(w-cx), random()%(h-cy), 0, 0, SWP_NOSIZE);
	}
		
#endif

#ifdef RANDOMPOP
	BlockInput(TRUE);
	for(int i = 0; i < 13; i++){
		LPCWSTR lpDescription = (random()%3==0)?lpPaths[i]:NULL;
		ReportError(lpPaths[i], L"确定程序嗝屁", lpDescription);
		Sleep(50);
		HWND hwnd = GetForegroundWindow();
		RECT rc;
		if(hwnd) {
			GetWindowRect(hwnd, &rc);
			cx = rc.right - rc.left, cy = rc.bottom - rc.top;
			SetWindowPos(hwnd, HWND_TOPMOST, random()%(w-cx), random()%(h-cy), 0, 0, SWP_NOSIZE);
		}
		
		Sleep(50+random()%50);
	}
	BlockInput(FALSE);
#endif
	return 0;
}
