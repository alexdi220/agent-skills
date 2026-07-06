# Getting Started on .NET Framework — Not Supported

**`AIChatControl` is not available on .NET Framework.** This page exists for parity with the repo's two-variant getting-started convention; the actual setup is in [getting-started.md](getting-started.md) (.NET 8+).

## Why

`AIChatControl` (`DevExpress.AIIntegration.WinForms.Chat`) is a Blazor/WebView2-hosted control. It requires:

- **.NET 8 or newer** targeting Windows
- a project using the **`Microsoft.NET.Sdk.Razor`** SDK
- the WebView2 runtime at runtime

None of these are available to classic **.NET Framework 4.x** projects, so the control cannot be added to a .NET Framework app.

## What to Do Instead

- **Migrate the app (or a chat-hosting window) to .NET 8+** and follow [getting-started.md](getting-started.md). This is the only way to use `AIChatControl`.
- **If you must stay on .NET Framework**, build a custom chat UI with standard WinForms/DevExpress editors (e.g., a `MemoEdit`/`RichEditControl` transcript plus a `TextEdit` input) and call your AI provider's SDK directly (`Microsoft.Extensions.AI` / `OpenAI` / `Azure.AI.OpenAI`). The DevExpress AI-powered Extensions chat samples show provider-client wiring you can reuse: https://github.com/DevExpress-Examples/devexpress-ai-chat-samples

## See Also

- [getting-started.md](getting-started.md) — the supported .NET 8+ setup
- SKILL.md → Troubleshooting: "Control not available on .NET Framework"
