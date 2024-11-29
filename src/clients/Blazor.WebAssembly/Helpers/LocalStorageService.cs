﻿using Blazored.LocalStorage;

namespace Blazor.WebAssembly.Helpers;

internal sealed class LocalStorageService(ILocalStorageService localStorageService)
{
    private const string StorageKey = "authentication-token";
    public async Task<string?> GetToken()
    {
        return await localStorageService.GetItemAsStringAsync(StorageKey);
    }

    public async Task SetToken(string item)
    {
        await localStorageService.SetItemAsStringAsync(StorageKey, item);
    }

    public async Task RemoveToken()
    {
        await localStorageService.RemoveItemAsync(StorageKey);
    }
}