#pragma once

#pragma warning(push)
#pragma warning(disable: 4302 4838)
#include <atlbase.h>
#include <atlfile.h>
#include <atlcoll.h>
#include <atlstr.h>
#include <atlwin.h>
#include <atlapp.h>
#include <atlctrls.h>
#include <atldlgs.h>
#pragma warning(pop)

#include "unzip.h"
#include "resource.h"
#include <shellapi.h>

static HINSTANCE hInstance;
static void ShowFailureDialog(const CString& mainInstruction, const CString& content = _T("")) {
	CTaskDialog td;
	td.SetMainInstructionText(mainInstruction);
	if (content.GetLength() != 0) td.SetContentText(content);
	td.SetWindowTitle(_T("Chocolatey Installer"));
	td.SetCommonButtons(TDCBF_CLOSE_BUTTON);
	td.SetMainIcon(TD_ERROR_ICON);
	td.ModifyFlags(0, TDF_ALLOW_DIALOG_CANCELLATION);
	td.DoModal(HWND_DESKTOP);
}

static bool ExtractZip(WORD zipResourceId, const CString& directory) {
	CResource zipResource;
	if (!zipResource.Load(_T("ZIPFILE"), zipResourceId)) return false;

	unsigned int dwSize = zipResource.GetSize();
	if (dwSize < 0x100) return false;

	HZIP zipFile = OpenZip(zipResource.Lock(), dwSize, NULL);
	SetUnzipBaseDir(zipFile, directory.GetString());

	ZRESULT zr; int index = 0; bool result = true;
	do {
		ZIPENTRY zipEntry;

		zr = GetZipItem(zipFile, index, &zipEntry);
		if (zr != ZR_MORE && zr != ZR_OK) break;

		CString targetFile = directory + "\\" + zipEntry.name;
		DeleteFile(targetFile.GetString());

		ZRESULT zr_inner = UnzipItem(zipFile, index, zipEntry.name);
		if (zr_inner != ZR_OK) {
			// Sometimes ZR_FLATE can be returned incorrectly if the file being
			// decompressed is of zero length. (UnzipItem() cannot differentiate
			// between a genuine zero-size read and an error condition.) Only fail
			// if the size decompressed size is not actually zero.
			if (zr_inner == ZR_FLATE && zipEntry.unc_size != 0) {
				result = false;
				break;
			}
		}

		index++;
	} while (zr == ZR_MORE || zr == ZR_OK);

	CloseZip(zipFile);
	zipResource.Release();

	return result;
}

#define REENTRANCY_MUTEX_NAME _T("Local\\Chocolatey Installer Reentrancy Mutex")

int WINAPI _tWinMain(HINSTANCE hInst, HINSTANCE /* hPrevInstance */, LPTSTR cmdLine, INT nCmdShow) {
	hInstance = hInst;

	HANDLE hMutex = ::OpenMutex(SYNCHRONIZE, false, REENTRANCY_MUTEX_NAME);
	bool alreadyExists = (hMutex == NULL && GetLastError() == ERROR_FILE_NOT_FOUND);

	if (!alreadyExists) {
		CString mainInstruction; mainInstruction.LoadStringW(hInstance, IDS_REENTRANCY);
		CString content; content.LoadStringW(hInstance, IDS_REENTRANCY_DESC);
		ShowFailureDialog(mainInstruction, content);
		CloseHandle(hMutex);
		return 1;
	}

	hMutex = ::CreateMutex(nullptr, true, REENTRANCY_MUTEX_NAME);

	TCHAR targetDir[MAX_PATH];
	if (!::GetEnvironmentVariable(_T("TEMP"), targetDir, MAX_PATH)) {
		CString mainInstruction; mainInstruction.LoadStringW(hInstance, IDS_NOTEMPDIR);
		CString content; content.LoadStringW(hInstance, IDS_NOTEMPDIR_DESC);
		ShowFailureDialog(mainInstruction, content);
		return 1;
	}

	_tcscat_s(targetDir, _T("\\ChocolateyInstaller"));
	if (!::CreateDirectory(targetDir, nullptr) && ::GetLastError() != ERROR_ALREADY_EXISTS) {
		CString mainInstruction; mainInstruction.LoadStringW(hInstance, IDS_TEMPDIRWRITEFAIL);
		CString content; content.LoadStringW(hInstance, IDS_TEMPDIRWRITEFAIL_DESC);
		ShowFailureDialog(mainInstruction, content);
		return 1;
	}

	CString outputDir = targetDir;
	const WORD IDR_CHOCOLATEY_NUPKG = 1, IDR_WIZARD_ZIP = 2;
	if (!ExtractZip(IDR_CHOCOLATEY_NUPKG, outputDir + "\\choco_nupkg")) {
		CString mainInstruction; mainInstruction.LoadStringW(hInstance, IDS_EXTRACTFAIL);
		ShowFailureDialog(mainInstruction, _T(""));
		return 1;
	}

	if (!ExtractZip(IDR_WIZARD_ZIP, outputDir)) {
		CString mainInstruction; mainInstruction.LoadStringW(hInstance, IDS_EXTRACTFAIL);
		ShowFailureDialog(mainInstruction, _T(""));
		return 1;
	}

	CString cmd = outputDir + "\\ChocolateyInstaller.Wizard.exe";
	STARTUPINFO si; PROCESS_INFORMATION pi;
	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(STARTUPINFO);
	si.wShowWindow = SW_SHOW;
	si.dwFlags = STARTF_USESHOWWINDOW;

	if (!CreateProcess(cmd.GetString(), _T(""), nullptr, nullptr, false, 0, nullptr, outputDir.GetString(), &si, &pi)) {
		CString mainInstruction; mainInstruction.LoadStringW(hInstance, IDS_EXECFAIL);
		ShowFailureDialog(mainInstruction, _T(""));
		return -1;
	}

	WaitForSingleObject(pi.hProcess, INFINITE);
	CloseHandle(hMutex);
	return 0;
}