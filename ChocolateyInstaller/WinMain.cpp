#pragma once

#pragma warning(push)
#pragma warning(disable: 4302 4838)
#include <atlbase.h>
#include <atlstr.h>
#include <atlwin.h>
#include <atlapp.h>
#include <atlctrls.h>
#include <atldlgs.h>
#pragma warning(pop)

#include "unzip.h"
#include "resource.h"

int WINAPI _tWinMain(HINSTANCE hInstance, HINSTANCE /* hPrevInstance */, LPTSTR cmdLine, INT nCmdShow) {
	CTaskDialog td;
	td.SetMainInstructionText(_T("This application is not yet implemented."));
	td.SetWindowTitle(_T("Chocolatey Installer"));
	td.SetCommonButtons(TDCBF_CLOSE_BUTTON);
	td.SetMainIcon(TD_ERROR_ICON);
	td.ModifyFlags(0, TDF_ALLOW_DIALOG_CANCELLATION);
	td.DoModal(HWND_DESKTOP);

	return 1;
}