#ifndef UNICODE
#define UNICODE
#endif

#ifndef _UNICODE
#define _UNICODE
#endif

#include <stdlib.h>
#include <stdio.h>
#include <windows.h>

int wmain(int argc, LPCWSTR *argv)
{
    LPCWSTR formatLink = L"http://www.erlang.org/doc/man/%s.html#%s";
    WCHAR moduleName[MAX_PATH], functionName[MAX_PATH];
    LPWSTR link;
    DWORD res;
    int linkLen, i;

    switch(argc)
    {
    case 2:
        wcscpy_s(moduleName, ARRAYSIZE(moduleName), argv[1]);
        memset(functionName, 0, sizeof(functionName));
        break;
    case 3:
        wcscpy_s(moduleName, ARRAYSIZE(moduleName), argv[1]);
        wcscpy_s(functionName, ARRAYSIZE(functionName), argv[2]);
        for(i = 0; functionName[i]; ++i)
        {
            if(functionName[i] == '/')
            {
                functionName[i] = '-';
                break;
            }
        }
        break;
    default:
        fwprintf(stderr, L"usage: %s <module> [<function>]\n", argv[0]);
        return 1;
    }

    linkLen = _scwprintf(formatLink, moduleName, functionName);
    if(linkLen <= 0)
    {
        fwprintf(stderr, L"Unable to format link\n");
        return 1;
    }
    link = (LPWSTR)calloc(linkLen + 1, sizeof(link[0]));
    if(!link)
    {
        fwprintf(stderr, L"Unable to format link\n");
        return 1;
    }
    linkLen = swprintf_s(link,
                         linkLen + 1,
                         formatLink,
                         moduleName,
                         functionName);
    if(linkLen < 0)
    {
        free(link);
        fwprintf(stderr, L"Unable to format link\n");
        return 1;
    }

    res = (DWORD)ShellExecuteW(NULL, NULL, link, NULL, NULL, SW_SHOWNORMAL);
    if(res < 32)
    {
        fwprintf(stderr, L"Unable open link: %u", res);
    }
    free(link);
    return 0;
}