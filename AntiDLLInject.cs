﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Loader.Security.AntiDLLInject
{
    class AntiDLLInject
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lib);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr ModuleHandle, string Function);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr ProcHandle, IntPtr BaseAddress, byte[] Buffer, uint size, int NumOfBytes);
        public static string PatchLoadLibraryA()
        {
            IntPtr KernelModule = GetModuleHandle("kernelbase.dll");
            IntPtr LoadLibraryA = GetProcAddress(KernelModule, "LoadLibraryA");
            byte[] HookedCode = { 0xC2, 0x04, 0x00 };
            bool Status = WriteProcessMemory(Process.GetCurrentProcess().Handle, LoadLibraryA, HookedCode, 3, 0);
            if (Status)
                return "Success";
            return "Failed";
        }

        public static string PatchLoadLibraryW()
        {
            IntPtr KernelModule = GetModuleHandle("kernelbase.dll");
            IntPtr LoadLibraryW = GetProcAddress(KernelModule, "LoadLibraryW");
            byte[] HookedCode = { 0xC2, 0x04, 0x00 };
            bool Status = WriteProcessMemory(Process.GetCurrentProcess().Handle, LoadLibraryW, HookedCode, 3, 0);
            if (Status)
                return "Success";
            return "Failed";
        }
        public static unsafe void InitializeDLLInject()
        {
            if (PatchLoadLibraryA() != "Success")
            {
                Environment.FailFast(null);
            }
            if (PatchLoadLibraryW() != "Success")
            {
                Environment.FailFast(null);
            }

        }
    }
}
