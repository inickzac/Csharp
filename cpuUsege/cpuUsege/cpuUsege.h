#pragma once
#ifdef CPUUSEGE_EXPORTS
#define CPUUSEGE_API __declspec(dllexport)
#else
#define CPUUSEGE_API __declspec(dllimport)
#endif

extern "C" CPUUSEGE_API double get_cpu_usege();