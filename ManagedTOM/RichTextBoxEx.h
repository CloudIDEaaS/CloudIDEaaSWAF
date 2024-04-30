// RichTextBoxEx.h

#pragma once

#include "Stdafx.h"
#include <windows.h>
#include <msclr\marshal.h> 

using namespace System;
using namespace System::IO;
using namespace System::ComponentModel;
using namespace System::Collections::Generic;
using namespace System::Windows::Forms;
using namespace Microsoft::Win32;
using namespace msclr::interop;

namespace TextObjectModel {

	/// <summary>
	/// Extends <see cref="RichTextBox"/> to allow the underlying RichEdit 
	/// control to be loaded from either the shared Microsoft Office DLL or 
	/// the Windows MSFTEDIT.DLL library.
	/// </summary>
	/// <remarks>
	/// <para>
	/// During its initial creation, the control will attempt to locate a 
	/// Microsoft Office shared DLL containing the desired RichEdit control. 
	/// If this fails (either because Office is not installed or the DLL is 
	/// for a different processor architecture), it will try MSFTEDIT.DLL 
	/// instead.
	/// </para>
	/// <para>
	/// In order to utilise the Microsoft Office implementation, the caller 
	/// must have read access for following registry key: 
	/// HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDLLs - this 
	/// may not be available in sandboxed environments.
	/// </para>
	/// <para>
	/// Important: The processor architecture of the Microsoft Office 
	/// installation must match that of the calling process. If both the 32-bit 
	/// and 64-bit versions of Office are installed, the control will attempt 
	/// both DLLs until it finds a suitable match.
	/// </para>
	/// <para>
	/// After loading the libraries into memory, it will then select the 
	/// highest version of the RichEdit control that is available. It will 
	/// select from the following window class names (in order of preference): 
	/// </para>
	/// <list type="bullet">
	/// <item><description>RICHEDIT60W (RichEdit 6.0 / Office 2007 and higher)</description></item>
	/// <item><description>RICHEDIT50W (RichEdit 4.1)</description></item>
	/// <item><description>RICHEDIT20W (RichEdit 2.0)</description></item>
	/// </list>
	/// </remarks>
	[ToolboxItem(true), DisplayName("RichTextBox (Extended)")]
	[Description("Provides advanced rich text editing functionality.")]
	public ref class RichTextBoxEx : public RichTextBox {

	public:

		/// <summary>
		/// Type initializer.
		/// </summary>
		static RichTextBoxEx() {
			_overrideSearched = false;
			_overrideWindowClassName = nullptr;
		}

		/// <summary>
		/// Creates a <see cref="RichTextBoxEx"/> by loading a specific version 
		/// of the native RichEdit control from the specified DLL.
		/// </summary>
		/// <param name="fileName">The path to the DLL that contains the RichEdit control.</param>
		/// <param name="windowClassName">The name of the window class used to create the control.</param>
		/// <returns>The control.</returns>
		/// <exception cref="DllNotFoundException">
		/// The DLL at the path pointed to by <paramref name="fileName"/> does 
		/// not exist, could not be loaded or has the wrong processor architecture.
		/// </exception>
		static RichTextBoxEx^ LoadFrom(String^ fileName, String^ windowClassName) {
			_overrideWindowClassName = windowClassName;
			_overrideSearched = true;
			
			try {
				marshal_context^ context = gcnew marshal_context();
				LPCSTR str = context->marshal_as<LPCSTR>(fileName);
				if (LoadLibrary(str) == NULL) {
					DWORD errorCode = GetLastError();
					throw gcnew DllNotFoundException(GetErrorMessage(errorCode));
				}

				return gcnew RichTextBoxEx();
			}
			finally {
				_overrideSearched = false;
				_overrideWindowClassName = nullptr;
			}
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="RichTextBoxEx"/> class.
		/// </summary>
		RichTextBoxEx() { }

		/// <summary>
		/// Gets the required creation parameters when the control handle is created.
		/// </summary>
		[Browsable(false)]
		virtual property System::Windows::Forms::CreateParams^ CreateParams {
			System::Windows::Forms::CreateParams^ get() override {
				if (_overrideSearched) {
					// used by the LoadFrom method
					_searched = true;
					_windowClassName = _overrideWindowClassName;
				}

				if (!_searched && !DesignMode) {
					SortedList<Version^, String^>^ msoDllPaths = gcnew SortedList<Version^, String^>();
					_windowClassName = String::Empty;
					marshal_context^ context = gcnew marshal_context();

					// look for MS Office RichEdit DLL
					RegistryKey^ key;
					try {
						key = Registry::LocalMachine->OpenSubKey(sharedDllRegKey);
						array<String^>^ valueNames = key->GetValueNames();
						bool found = false;

						for (int i=0; i<valueNames->Length; i++) {
							String^ path = valueNames[i];

							if (Path::GetFileName(path)->Equals(msoDllName, System::StringComparison::OrdinalIgnoreCase)) {
								// get DLL version
								Version^ dllVersion = GetDllVersion(path);
								
								if (dllVersion != nullptr) {
									found = true;
									msoDllPaths->Add(dllVersion, path);
#ifdef _DEBUG
									Console::WriteLine("RichEdit DLL ({0}) found at {1}", dllVersion, path);
#endif
								}
								else {
#ifdef _DEBUG
									Console::WriteLine("Unable to read version number from {0}", path);
#endif
								}
							}
						}

#ifdef _DEBUG							
						if (!found) {
							Console::WriteLine("No RichEdit DLL found in {0}", sharedDllRegKey);
						}
#endif
					}
					catch (System::Security::SecurityException^ ex) {
						// read access denied, log error and continue
#ifdef _DEBUG							
						Console::WriteLine("Registry access was denied ({0}: {1})", ex->GetType()->Name, ex->Message);
#endif
					}
					finally {
						delete key;
					}

					// fallback DLL (still newer than the default)
					msoDllPaths->Add(gcnew Version(1,0), msftDllName);

					// try each window class name in order of preference
					for each (String^ className in classNames) {
						bool windowClassFound = false;

						// try each DLL (highest version first)
						for (int i=(msoDllPaths->Count - 1); i>=0; i--) {
							String^ path = msoDllPaths->Values[i];
							const char* msoDllPathCStr = context->marshal_as<const char*>(path);

							// load DLL
							HMODULE hModule = LoadLibrary(msoDllPathCStr);

							if (hModule != NULL) {
								// see if the window class has been registered
								const char* classNameCStr = context->marshal_as<const char*>(className);
								WNDCLASS info;
								BOOL result = GetClassInfo(NULL, classNameCStr, &info);

								if (result) {
									_windowClassName = className;
									windowClassFound = true;
#ifdef _DEBUG
									Console::WriteLine("Using {0} from {1}", className, path);
#endif
									break;
								}
								else {
#ifdef _DEBUG
									Console::WriteLine("{0} not found in {1}", className, path);
#endif
									// unload DLL
									FreeLibrary(hModule);
								}
							}
							else {
#ifdef _DEBUG
								// show error info
								DWORD errorCode = GetLastError();								
								Console::WriteLine("Failed to load {0} - {1}", path, GetErrorMessage(errorCode));
#endif
							}
						}

						// don't need to check other classes if we got a match
						if (windowClassFound) break;
					}

					_searched = true;
					delete context;
				}				
				
				System::Windows::Forms::CreateParams^ cp = RichTextBox::CreateParams;
				if (!String::IsNullOrEmpty(_windowClassName)) cp->ClassName = _windowClassName;
				return cp;
			}
		}

	private:

		// Returns a string containing the error message for the specified error code.
		static String^ GetErrorMessage(DWORD errorCode) {
			LPTSTR str = NULL;
			
			// transform the error code into an error message
			DWORD result = FormatMessage(
				FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_ALLOCATE_BUFFER, NULL, 
				errorCode, 
				0, 
				(LPTSTR)&str, 
				0, 
				NULL
			);

			return (result != 0) ? gcnew String(str) : "File not found, or wrong image format.";
		}

		// Returns the file version of the specified DLL.
		Version^ GetDllVersion(String^ path) {
			marshal_context^ context = gcnew marshal_context();
			LPCSTR szVersionFile = context->marshal_as<LPCSTR>(path);
			Version^ version = nullptr;
			DWORD  verHandle = NULL;
			UINT   size      = 0;
			LPBYTE lpBuffer  = NULL;
			DWORD  verSize   = GetFileVersionInfoSize(szVersionFile, &verHandle);

			if (verSize != NULL) {
				LPSTR verData = new char[verSize];

				if (GetFileVersionInfo(szVersionFile, verHandle, verSize, verData)) {
					if (VerQueryValue(verData,"\\",(VOID FAR* FAR*)&lpBuffer,&size)) {
						if (size) {
							VS_FIXEDFILEINFO *verInfo = (VS_FIXEDFILEINFO *)lpBuffer;

							if (verInfo->dwSignature == 0xfeef04bd) {
								// Doesn't matter if you are on 32 bit or 64 bit,
								// DWORD is always 32 bits, so first two revision numbers
								// come from dwFileVersionMS, last two come from dwFileVersionLS
								version = gcnew Version(
									(verInfo->dwFileVersionMS >> 16 ) & 0xffff,
									(verInfo->dwFileVersionMS >>  0 ) & 0xffff,
									(verInfo->dwFileVersionLS >> 16 ) & 0xffff,
									(verInfo->dwFileVersionLS >>  0 ) & 0xffff
								);
							}
						}
					}
				}

				delete[] verData;
			}

			delete context;

			return version;
		}

		// Registry key for Shared DLLs.
		literal String^ sharedDllRegKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\SharedDLLs";
		
		// Name of the RichEdit DLL that ships with Windows.
		literal String^ msftDllName = "msftedit.dll";

		// Name of the RichEdit DLL that ships with Office.
		literal String^ msoDllName = "riched20.dll";

		// Array containing the known window class names of the RichEdit control, in order of preference.
		static initonly array<String^>^ classNames = { "RICHEDIT60W", "RICHEDIT50W", "RICHEDIT20W" };

		static String^ _overrideWindowClassName;
		static bool _overrideSearched;

		String^ _windowClassName;
		bool _searched;

	};
}
