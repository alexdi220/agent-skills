// DevExpress WinForms AI Chat (AIChatControl) — Quickstart (C#)
// Demonstrates: client registration, hosting the control, streaming, header,
//               manual message interception (MessageSending), save/restore history.
// (For Markdown rendering, see the "Markdown rendering" pattern in SKILL.md —
//  it needs Markdig/HtmlSanitizer and a MarkdownConvert handler.)
// Packages: DevExpress.AIIntegration.WinForms.Chat (+ an AI provider, e.g. OpenAI)
// Requires: .NET 8+, <Project Sdk="Microsoft.NET.Sdk.Razor">

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.AIIntegration;
using DevExpress.AIIntegration.WinForms.Chat;
using DevExpress.XtraEditors;
using Microsoft.Extensions.AI;

// ------------------------------------------------------------------
// 1. Register the chat client at startup (Program.cs)
// ------------------------------------------------------------------
static class Program {
    [STAThread]
    static void Main() {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        IChatClient chatClient = new OpenAI.OpenAIClient(
                Environment.GetEnvironmentVariable("OPENAI_API_KEY"))
            .GetChatClient("gpt-4o-mini")
            .AsIChatClient();

        AIExtensionsContainerDesktop.Default.RegisterChatClient(chatClient);
        Application.Run(new Form1());
    }
}

// ------------------------------------------------------------------
// 2. Host the control
// ------------------------------------------------------------------
// Designer-free so this file compiles as a single-file quickstart.
public class Form1 : XtraForm {
    public Form1() {
        Text = "AI Chat";

        var aiChatControl1 = new AIChatControl { Dock = DockStyle.Fill };
        Controls.Add(aiChatControl1);

        // Streaming responses
        aiChatControl1.UseStreaming = DevExpress.Utils.DefaultBoolean.True;

        // Header
        aiChatControl1.ShowHeader = DevExpress.Utils.DefaultBoolean.True;
        aiChatControl1.HeaderText = "AI Assistant";

        // Optional: intercept messages before they are sent (see section 3)
        ManualInterception.Wire(aiChatControl1);
    }
}

// ------------------------------------------------------------------
// 3. Intercept messages manually (MessageSending)
//    Handle MessageSending to inspect/modify a user message before it is
//    sent. Append messages with e.Chat.AppendMessageAsync(...); set
//    e.Cancel = true to bypass the default AI call and answer yourself.
// ------------------------------------------------------------------
public static class ManualInterception {
    // Wire the MessageSending event on an existing AIChatControl instance.
    public static void Wire(AIChatControl chat) {
        chat.MessageSending += async (s, e) => {
            // Add a system instruction before the user message is sent.
            await e.Chat.AppendMessageAsync("Translate text to Spanish", ChatRole.System);

            // To bypass the default AI service and reply yourself:
            // e.Cancel = true;
            // await e.Chat.AppendMessageAsync(await MyService.AskAsync("..."), ChatRole.Assistant);
        };
    }
}

static class MyService {
    public static System.Threading.Tasks.Task<string> AskAsync(string q) =>
        System.Threading.Tasks.Task.FromResult("...");
}

// ------------------------------------------------------------------
// 4. Save and restore chat history
// ------------------------------------------------------------------
public static class History {
    public static void Demo(AIChatControl chat) {
        var saved = (List<DevExpress.AIIntegration.Blazor.Chat.BlazorChatMessage>)chat.SaveMessages();
        // ... later ...
        chat.LoadMessages(saved);
    }
}
