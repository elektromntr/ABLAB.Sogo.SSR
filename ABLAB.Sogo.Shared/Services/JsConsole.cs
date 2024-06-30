﻿using Microsoft.JSInterop;

namespace ABLAB.Sogo.Shared.Services
{
    public class JsConsole
    {
        private readonly IJSRuntime JsRuntime;
        public JsConsole(IJSRuntime jSRuntime)
        {
            this.JsRuntime = jSRuntime;
        }

        public async Task LogAsync(string message)
        {
            await this.JsRuntime.InvokeVoidAsync("console.log", message);
        }

        public async Task LogComponent(string name)
        {
            await LogAsync($"{name} initialized");
        }
    }
}
