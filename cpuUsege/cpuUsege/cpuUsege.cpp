#include "pch.h"
#include <iostream>
#include <windows.h>
#include <Winternl.h> 
#include <thread>
#include <cstdio>
#include <windows.h>
#include "cpuUsege.h"

typedef BOOL(__stdcall* GetSystemTimes_API_ptr)(LPFILETIME, LPFILETIME, LPFILETIME);
double get_cpu_usege();

double filetime2double_msec(FILETIME ft) {
	return 1e-4 * ((UINT64(ft.dwHighDateTime) << 32) + (UINT64)ft.dwLowDateTime);
}

FILETIME IdleTime1, IdleTime2, KernelTime1, KernelTime2, UserTime1, UserTime2;
HINSTANCE hLib = LoadLibraryA("kernel32.dll");
GetSystemTimes_API_ptr GetSystemTimes_API =
(GetSystemTimes_API_ptr)GetProcAddress(hLib, "GetSystemTimes");


double get_cpu_usege() {
	GetSystemTimes_API(&IdleTime1, &KernelTime1, &UserTime1);
	Sleep(75);
	GetSystemTimes_API(&IdleTime2, &KernelTime2, &UserTime2);
	FreeLibrary(hLib);
	double Overall = filetime2double_msec(KernelTime2) +
		filetime2double_msec(UserTime2) -
		filetime2double_msec(KernelTime1) -
		filetime2double_msec(UserTime1);
	double Busy = Overall -
		filetime2double_msec(IdleTime2) +
		filetime2double_msec(IdleTime1);
	return 100.0 * (Busy / Overall);
}
