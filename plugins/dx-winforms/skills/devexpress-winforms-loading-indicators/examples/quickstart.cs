// DevExpress WinForms Loading Indicators — Quickstart (C#)
// Demonstrates: overlay form over a control, wait form, fluent splash screen, ProgressPanel.
// Package: DevExpress.Win.Navigation   Host form: XtraForm

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors;
using DevExpress.XtraWaitForm;

// ------------------------------------------------------------------
// 1. Overlay form over a control while data loads (most common)
// ------------------------------------------------------------------
public partial class MainForm : XtraForm {
    Control gridControl1;

    // Close the startup Fluent splash (shown in Program.Main) once the form is visible.
    // The splash runs on its own thread, so a missing CloseForm() can keep the app alive on exit.
    protected override void OnShown(EventArgs e) {
        base.OnShown(e);
        SplashScreenManager.CloseForm();
    }

    async void btnRefresh_Click(object sender, EventArgs e) {
        IOverlaySplashScreenHandle handle = SplashScreenManager.ShowOverlayForm(gridControl1);
        try {
            await LoadDataAsync();
        }
        finally {
            SplashScreenManager.CloseOverlayForm(handle);
        }
    }

    Task LoadDataAsync() => Task.CompletedTask;
}

// ------------------------------------------------------------------
// 2. Wait form for a long background task
//    (requires a SplashScreenManager component + generated WaitForm1)
// ------------------------------------------------------------------
public partial class WaitForm_Demo : XtraForm {
    SplashScreenManager splashScreenManager1;

    async Task RunExport() {
        splashScreenManager1.ShowWaitForm();
        try {
            splashScreenManager1.SetWaitFormCaption("Exporting...");
            await ExportAsync();
        }
        finally {
            splashScreenManager1.CloseWaitForm();
        }
    }

    Task ExportAsync() => Task.CompletedTask;
}

// ------------------------------------------------------------------
// 3. Fluent splash screen at app startup (Program.cs)
// ------------------------------------------------------------------
static class Program {
    [STAThread]
    static void Main() {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        SplashScreenManager.ShowFluentSplashScreen(
            title: "My App",
            subtitle: "Loading...",
            loadingIndicatorType: FluentLoadingIndicatorType.Dots
        );
        // MainForm.OnShown calls SplashScreenManager.CloseForm() to close this splash.
        Application.Run(new MainForm());
    }
}

// ------------------------------------------------------------------
// 4. ProgressPanel embedded in a form
// ------------------------------------------------------------------
public static class ProgressPanelDemo {
    public static ProgressPanel Build() => new ProgressPanel {
        Caption = "Loading",
        Description = "Please wait...",
        WaitAnimationType = DevExpress.Utils.Animation.WaitingAnimatorType.Line,
        Dock = DockStyle.Fill
    };
}
