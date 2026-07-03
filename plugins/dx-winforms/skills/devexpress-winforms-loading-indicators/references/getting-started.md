# Getting Started

This reference covers which NuGet package to install, which assemblies and namespaces are involved, and the minimum code required to show each loading indicator type.

## NuGet Package

All loading indicator classes ship in:

```
DevExpress.Win.Navigation
```

| Class | Assembly | Namespace |
|---|---|---|
| `SplashScreenManager` | `DevExpress.XtraEditors.v26.1.dll` | `DevExpress.XtraSplashScreen` |
| `WaitForm` | `DevExpress.XtraEditors.v26.1.dll` | `DevExpress.XtraWaitForm` |
| `ProgressPanel` | `DevExpress.XtraEditors.v26.1.dll` | `DevExpress.XtraWaitForm` |
| Overlay Form (via `SplashScreenManager`) | `DevExpress.XtraEditors.v26.1.dll` | `DevExpress.XtraSplashScreen` |
| Fluent/Skin Splash Screen (via `SplashScreenManager`) | `DevExpress.XtraEditors.v26.1.dll` | `DevExpress.XtraSplashScreen` |

> If your project already references `DevExpress.Win` or `DevExpress.Win.Design`, no additional package is needed — those umbrella packages include `DevExpress.Win.Navigation`.

## Common Namespaces

```csharp
using DevExpress.XtraSplashScreen;   // SplashScreenManager, overlay form API
using DevExpress.XtraWaitForm;        // WaitForm base class, ProgressPanel
```

---

## Splash Screen — Minimum Code

The simplest modern splash screen uses the built-in Fluent template. No custom form required — one static method call shows it; another closes it.

```csharp
// In Program.cs — before Application.Run(new MainForm())
SplashScreenManager.ShowFluentSplashScreen(
    title:                "My Application",
    subtitle:             "Version 1.0",
    rightFooter:          "Starting...",
    leftFooter:           "© 2026 My Company",
    loadingIndicatorType: FluentLoadingIndicatorType.Dots,
    useFadeIn:            true,
    useFadeOut:           true
);

Application.Run(new MainForm());

// In MainForm's constructor or Form_Load — after initialization completes:
SplashScreenManager.CloseForm();
```

---

## Wait Form — Minimum Code

1. Drop a `SplashScreenManager` component on the form.
2. Right-click it in the VS tray → **Add Wait Form**. A `WaitForm1` class is generated.
3. Show/hide via the `SplashScreenManager` instance or the static API:

```csharp
// Show (opens in a separate thread — non-blocking)
splashScreenManager1.ShowWaitForm();

// Optionally update labels
splashScreenManager1.SetWaitFormCaption("Loading data...");
splashScreenManager1.SetWaitFormDescription("Fetching from server");

// ... do work ...

// Close
splashScreenManager1.CloseWaitForm();
```

Static (no designer component needed):

```csharp
SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
// ... do work ...
SplashScreenManager.CloseForm();
```

---

## Overlay Form — Minimum Code

No custom form class required. Call `ShowOverlayForm`, get a handle, and close it when done:

```csharp
using DevExpress.XtraSplashScreen;

private async void btnLoad_Click(object sender, EventArgs e)
{
    IOverlaySplashScreenHandle handle = SplashScreenManager.ShowOverlayForm(gridControl1);
    try {
        await LoadDataAsync();
    }
    finally {
        SplashScreenManager.CloseOverlayForm(handle);
    }
}
```

The overlay can target any control or the form itself.

---

## ProgressPanel — Minimum Code (Inline in Form)

`ProgressPanel` is a regular control you place directly on a form or user control. No separate thread required.

```csharp
using DevExpress.XtraWaitForm;

// In designer: drag ProgressPanel from Toolbox, set Visible = false
// In code:
private async void btnLoad_Click(object sender, EventArgs e)
{
    progressPanel1.Caption     = "Loading";
    progressPanel1.Description = "Please wait...";
    progressPanel1.Visible     = true;
    progressPanel1.BringToFront();

    try {
        await LoadDataAsync();
    }
    finally {
        progressPanel1.Visible = false;
    }
}
```

Or create it in code:

```csharp
var pp = new ProgressPanel {
    Caption            = "Loading",
    Description        = "Please wait...",
    WaitAnimationType  = DevExpress.Utils.Animation.WaitingAnimatorType.Ring,
    Dock               = DockStyle.Fill
};
Controls.Add(pp);
pp.BringToFront();
```

---

## Design-Time Quick Start

| Indicator | Toolbox item | Steps |
|---|---|---|
| Splash Screen (Fluent/Skin) | None — code only | Call `SplashScreenManager.ShowFluentSplashScreen(...)` before `Application.Run()` |
| Wait Form | Drop **SplashScreenManager** onto form | Right-click component → Add Wait Form; call `ShowWaitForm()` / `CloseWaitForm()` |
| Overlay Form | None — code only | Call `SplashScreenManager.ShowOverlayForm(control)` |
| ProgressPanel | **ProgressPanel** in Toolbox | Drag onto form; set `Visible = false`; toggle in code |

## Source Material

- Splash Screen Manager: `https://docs.devexpress.com/content/WindowsForms/10826?md=true`
- Overlay Form: `https://docs.devexpress.com/content/WindowsForms/120029?md=true`
- Wait Form: `https://docs.devexpress.com/content/WindowsForms/10824?md=true`
- `SplashScreenManager` class: `xref:DevExpress.XtraSplashScreen.SplashScreenManager`
- `ProgressPanel` class: `xref:DevExpress.XtraWaitForm.ProgressPanel`
