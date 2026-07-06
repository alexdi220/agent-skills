# Getting Started with AccordionControl (.NET Framework 4.6.2+)

This guide is for **.NET Framework** projects. For .NET 8+, see [getting-started.md](getting-started.md). Once the project is configured, all `AccordionControl` code (elements, groups, view modes, search, customization) is identical on both platforms.

## System Requirements

- .NET Framework 4.6.2 or newer, targeting Windows
- Visual Studio 2022+ (Visual Studio 2019 also supported)
- DevExpress WinForms subscription with the [Unified Component Installer](https://www.devexpress.com/Products/Try/) for the designer-first workflow, or DevExpress NuGet packages from nuget.org
- A valid DevExpress license

## Two Installation Paths

1. **Unified Component Installer** (recommended for designer-first workflows). Run the installer, then drag `AccordionControl` from the Visual Studio Toolbox onto a form. References are added automatically.
2. **NuGet package** (recommended for source control and CI builds). Install `DevExpress.Win.Navigation` from nuget.org.

> **Adding the DevExpress reference — do it the right way (especially when none exists yet).** Add the reference via the **NuGet Package Manager** (`Install-Package DevExpress.Win.Navigation`) or by **dropping an `AccordionControl` from the Toolbox onto a form once** (which initializes the assemblies and adds the reference automatically). Do **not** hand-edit a legacy (non-SDK) `.csproj` to add `<Reference>` entries, and do **not** copy DevExpress DLLs with shell/PowerShell commands — the non-SDK project format is intricate and manual edits routinely leave the project unable to build or run. If you cannot run the NuGet command and no DevExpress reference is present yet, ask the developer to install the package (or drop the control from the Toolbox once) instead of editing project files by hand.

## Path 2: NuGet Package

```powershell
Install-Package DevExpress.Win.Navigation
```

`PackageReference` form (keep every DevExpress package on the same version):

```xml
<ItemGroup>
  <PackageReference Include="DevExpress.Win.Navigation" Version="26.1.*" />
</ItemGroup>
```

> If your project still uses `packages.config`, the `Install-Package` command works the same — Visual Studio installs the package and adds the assembly references.

This package ships `DevExpress.XtraBars.v<version>.dll`, which contains the entire `DevExpress.XtraBars.Navigation` namespace.

## Required Assemblies (Manual Reference)

If you reference assemblies directly (no NuGet, no installer), add references from `C:\Program Files\DevExpress {version}\Components\Bin\Framework\` (replace `<version>` with your DevExpress version, e.g., `26.1`):

- `DevExpress.XtraBars.v<version>.dll` (`AccordionControl`, navigation elements)
- `DevExpress.XtraEditors.v<version>.dll`, `DevExpress.Utils.v<version>.dll`, `DevExpress.Data.v<version>.dll` (core dependencies)

Prefer the NuGet package over manual references so all DevExpress assemblies stay on one version.

## Required Namespace Imports

```csharp
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;       // XtraForm
using DevExpress.XtraBars.Ribbon;   // RibbonForm (optional)
```

## Host Form

Host `AccordionControl` on `XtraForm` (or `RibbonForm` / `FluentDesignForm`) for correct skin-aware rendering — a plain `Form` loses theming. Apply the application skin once at startup, before any form is shown.

## Minimal Setup in Code

The control API is identical to the .NET 8+ guide:

```csharp
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using System.Windows.Forms;

public partial class MainForm : XtraForm {
    public MainForm() {
        InitializeComponent();

        var accordion = new AccordionControl { Dock = DockStyle.Left };
        Controls.Add(accordion);

        var grpContacts = new AccordionControlElement(ElementStyle.Group) {
            Text = "Contacts", Expanded = true
        };
        var itemCustomers = new AccordionControlElement(ElementStyle.Item) { Text = "Customers" };
        grpContacts.Elements.Add(itemCustomers);
        accordion.Elements.Add(grpContacts);
    }
}
```

## .NET Framework–Specific Gotchas

- **Platform architecture**: 64-bit .NET Framework targets need 64-bit DevExpress assemblies — they ship in the same location.
- **MSBuild Targets**: The installer path registers MSBuild `.targets` files that resolve version-specific assembly references (for example, `DevExpress.XtraBars.v26.1.dll`) automatically. Without the installer (NuGet only), make sure project references use real version strings.
- **License file**: .NET Framework projects must include the `licenses.licx` file in the project (the installer adds it automatically). NuGet-only projects need to add it manually — see the DevExpress licensing documentation.

## What to Learn Next

The remaining references in this skill ([data-binding.md](data-binding.md), [items-and-customization.md](items-and-customization.md), [view-modes.md](view-modes.md), [search.md](search.md), [when-to-use.md](when-to-use.md)) apply identically to both .NET and .NET Framework once the project is configured.
