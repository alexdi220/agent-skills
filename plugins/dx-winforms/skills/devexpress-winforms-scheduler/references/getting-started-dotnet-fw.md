# Getting Started with the DevExpress WinForms Scheduler (.NET Framework 4.6.2+)

This guide is for **.NET Framework** projects. For .NET 8+, see [getting-started.md](getting-started.md). Once the project is configured, every other reference in this skill applies identically to both target frameworks.

## System Requirements

- .NET Framework 4.6.2 or newer (Windows)
- Visual Studio 2022+ (2019 also supported)
- DevExpress WinForms subscription via the [Unified Component Installer](https://www.devexpress.com/Products/Try/), or DevExpress packages from nuget.org
- A valid DevExpress license

## Two Installation Paths

1. **Unified Component Installer** (designer-first). Run the installer, then create the project from the **DevExpress Template Gallery**, or drag `SchedulerControl` and `SchedulerDataStorage` from the toolbox onto an `XtraForm`. References are added automatically.
2. **NuGet package** (recommended for source control / CI):

   ```powershell
   Install-Package DevExpress.Win.Scheduler
   ```

## Designer Workflow (.NET Framework)

1. Drop a `SchedulerControl` on an `XtraForm` (or `RibbonForm`); set `Dock = Fill`.
2. Drop a `SchedulerDataStorage` and assign it via the scheduler's smart tag.
3. Configure appointment/resource **field mappings** in the storage smart tag. The **Data Source Configuration Wizard / typed DataSets** are available in .NET Framework projects (not in .NET SDK projects) for bound mode.
4. Optionally drop a `DateNavigator` and set its `SchedulerControl` property.

## Required Assemblies (Manual Reference)

Prefer the NuGet package (it pulls all dependencies). If you reference assemblies directly, add (replace `26.1` with your version):

- `DevExpress.XtraScheduler.v26.1.dll` (the control)
- `DevExpress.XtraScheduler.v26.1.Core.dll` (scheduler engine)
- `DevExpress.XtraEditors.v26.1.dll`, `DevExpress.Utils.v26.1.dll`, `DevExpress.Data.v26.1.dll` (core dependencies)
- `DevExpress.Printing.v26.1.Core.dll` (only for print / export)

## Minimal Unbound Example

```csharp
using DevExpress.XtraScheduler;
using DevExpress.XtraEditors;
using System;

public partial class MainForm : XtraForm {
    SchedulerControl schedulerControl1;
    SchedulerDataStorage storage;
    public MainForm() {
        InitializeComponent();
        storage = new SchedulerDataStorage();
        schedulerControl1.DataStorage = storage;
        // Unbound mode: no field mappings needed — add appointments in code.
        schedulerControl1.Start = DateTime.Today;
    }
}
```

> Field mappings apply only to **bound** mode. In unbound mode (above), appointments are created in code with `storage.CreateAppointment(...)` — no mappings required.

See [getting-started.md](getting-started.md) for the full setup and [data-binding.md](data-binding.md) for bound mode with field mappings.
