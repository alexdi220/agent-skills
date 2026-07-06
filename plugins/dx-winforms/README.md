# AI Agent Skills for DevExpress WinForms Controls

This plugin contains AI agent skills for [DevExpress WinForms Controls](https://www.devexpress.com/products/net/controls/winforms/) (Windows Forms controls for .NET 8+ and .NET Framework 4.6.2+).

---

## Prerequisites

- .NET 8+ (`net8.0-windows`) or .NET Framework 4.6.2+ (Windows)
- DevExpress WinForms NuGet packages v26.1+ (feature-specific packages such as `DevExpress.Win.PivotGrid`, `DevExpress.Win.Navigation`, `DevExpress.Win.Charts`, and `DevExpress.Win.Scheduler`)
- [DevExpress subscription license](https://www.devexpress.com/buy/winforms-wpf-blazor-asp-net-maui/) (WinForms Subscription, DXperience Subscription, or Universal Subscription)
- A DevExpress form host (`XtraForm`, `RibbonForm`, or `FluentDesignForm`) recommended for consistent skinning
- The AI Chat skill additionally requires .NET 8+ and the `DevExpress.AIIntegration.WinForms.Chat` package

---

## Included AI Agent Skills

### Data-aware Controls

| Skill | Capabilities | Docs |
|---|---|---|
| [devexpress-winforms-data-grid](skills/devexpress-winforms-data-grid/) | `GridControl` (GridView, BandedGridView, AdvBandedGridView, CardView, LayoutView) and `TreeList` — data binding (regular, server, instant feedback, virtual), columns, in-place editing, conditional formatting, validation, filtering/search, sorting/grouping, master-detail, focus/selection, drag-and-drop, printing/exporting, and layout persistence | [Data Grid Documentation](https://docs.devexpress.com/WindowsForms/3455/controls-and-libraries/data-grid) |
| [devexpress-winforms-tree-list](skills/devexpress-winforms-tree-list/) | `TreeList` — bound mode (self-referential data via `KeyFieldName`/`ParentFieldName`), unbound mode (`AppendNode`, `BeginUnboundLoad`/`EndUnboundLoad`), virtual/on-demand loading (`HasChildren`, `BeforeExpand`), columns and in-place editors, sorting, filtering (ActiveFilterString, Find Panel), summaries, conditional formatting, node operations, drag-and-drop, printing and export | [TreeList Documentation](https://docs.devexpress.com/WindowsForms/2434/controls-and-libraries/tree-list) |
| [devexpress-winforms-pivot-grid](skills/devexpress-winforms-pivot-grid/) | `PivotGridControl` — bind data (in-memory, DataTable, EF/EF Core, server mode), arrange fields into row/column/data/filter areas, interval grouping (numeric ranges, date hierarchies), summaries, sorting, filtering, conditional formatting, view layout, and appearance customization | [Pivot Grid Documentation](https://docs.devexpress.com/WindowsForms/3409/controls-and-libraries/pivot-grid) |
| [devexpress-winforms-property-grid](skills/devexpress-winforms-property-grid/) | `PropertyGridControl` (VerticalGrid) — bind an object via `SelectedObject`, auto- and manually-defined rows (`EditorRow`/`CategoryRow`), in-place editors, categories and Office-style view tabs, expandable/nested complex properties, the built-in collection editor, the search/find panel, and value-change events | [Property Grid Documentation](https://docs.devexpress.com/WindowsForms/119885/controls-and-libraries/property-grid) |
| [devexpress-winforms-charts](skills/devexpress-winforms-charts/) | `ChartControl` — series and diagram types, data binding, axes (titles, labels, ranges), legends, selection, tooltips and crosshair, and aggregation/summary | [Chart Control Documentation](https://docs.devexpress.com/WindowsForms/8117/controls-and-libraries/chart-control) |
| [devexpress-winforms-scheduler](skills/devexpress-winforms-scheduler/) | `SchedulerControl` with `SchedulerDataStorage` — bound and unbound modes, appointment/resource field mappings, views (Day, Work Week, Full Week, Month, Year, Timeline, Agenda, Gantt), recurrence, reminders, labels, statuses, resource grouping, time scales and rulers, and appearance customization (custom draw, display text) | [Scheduler Documentation](https://docs.devexpress.com/WindowsForms/1971/controls-and-libraries/scheduler) |
| [devexpress-winforms-ai-chat](skills/devexpress-winforms-ai-chat/) | `AIChatControl` (`DevExpress.AIIntegration.WinForms.Chat`) — register an AI client (OpenAI / Azure OpenAI / Ollama), Markdown rendering (`ContentFormat`), message interception (`MessageSending`), prompt suggestions, and Razor message/content templates (`SetMessageTemplate`, `SetMessageContentTemplate`). Requires v26.1+ and .NET 8+ | [AI Chat Control Documentation](https://docs.devexpress.com/WindowsForms/405218/ai-powered-extensions/ai-chat-control) |

---

### Data Editors and Utilities

| Skill | Capabilities | Docs |
|---|---|---|
| [devexpress-winforms-editors](skills/devexpress-winforms-editors/) | DevExpress data editors — the `BaseEdit` hierarchy (`TextEdit`, `ButtonEdit`, `SpinEdit`, `DateEdit`, `ComboBoxEdit`, `LookUpEdit`, `MemoEdit`, `CheckEdit`, and more), `EditValue` binding, input masks (`MaskSettings`: numeric, datetime, regex, simple), editor buttons (`EditorButton` / `ButtonPredefines`), repository items for in-place editing, and non-`BaseEdit` controls (`HyperlinkLabelControl`, dialogs) | [Editors Documentation](https://docs.devexpress.com/WindowsForms/114580/controls-and-libraries/editors-and-simple-controls) |
| [devexpress-winforms-loading-indicators](skills/devexpress-winforms-loading-indicators/) | `SplashScreenManager` loading indicators — startup splash screens (Fluent / Skin / Default / Image), modal Wait Forms (`ShowWaitForm`, `SendCommand`/`ProcessCommand`), semi-transparent Overlay Forms (`ShowOverlayForm`), and the inline `ProgressPanel` — with the right async patterns | [Splash Screen Documentation](https://docs.devexpress.com/WindowsForms/10826/controls-and-libraries/forms-and-user-controls/splash-screen-manager) |

---

### Layout & Navigation

| Skill | Capabilities | Docs |
|---|---|---|
| [devexpress-winforms-layout](skills/devexpress-winforms-layout/) | `LayoutControl` / `DataLayoutControl` (aligned, self-arranging form layouts; auto-generate editors from a data source via `RetrieveFields`), `DockManager` (dockable/floating/auto-hide panels and layout persistence), and `StackPanel` / `TablePanel`. Designer-first guidance for authoring layouts in `*.Designer.cs` | [Layout & Docking Documentation](https://docs.devexpress.com/WindowsForms/5368/controls-and-libraries/docking-library) |
| [devexpress-winforms-ribbon-and-bars](skills/devexpress-winforms-ribbon-and-bars/) | `RibbonControl` and classic `BarManager` bars — `RibbonForm` host, items/links architecture (`BarButtonItem`, `BarCheckItem`, `BarSubItem`, `BarEditItem`, galleries), item visuals and icons (`ImageOptions.SvgImage`/`ImageUri`), ribbon structure (page categories, pages, groups, QAT, Application/Backstage menu), MDI ribbon/bars merging, and bar layout (main menu, toolbars, status bar) | [Ribbon Documentation](https://docs.devexpress.com/WindowsForms/1199/controls-and-libraries/ribbon-bars-and-menu) |
| [devexpress-winforms-accordion](skills/devexpress-winforms-accordion/) | `AccordionControl` — side navigation and hamburger-menu layouts, accordion elements (groups/items, `Hint` tooltips), view modes, content containers, data-driven population, customization, and built-in/AI smart search | [Accordion Documentation](https://docs.devexpress.com/WindowsForms/114553/controls-and-libraries/navigation-controls/accordion-control) |
| [devexpress-winforms-tab-control](skills/devexpress-winforms-tab-control/) | `XtraTabControl` — tab pages (`XtraTabPage`, add/remove, selection), header location and orientation, multi-line and scrolling headers, header buttons, page images, tab closing, wizard-style navigation, appearance and custom drawing | [Tab Control Documentation](https://docs.devexpress.com/WindowsForms/DevExpress.XtraTab.XtraTabControl) |

---

### Frameworks

| Skill | Capabilities | Docs |
|---|---|---|
| [devexpress-winforms-mvvm](skills/devexpress-winforms-mvvm/) | DevExpress WinForms MVVM — `MVVMContext`, ViewModel strategies (compile-time `[GenerateViewModel]` codegen and runtime POCO/`ViewModelBase`), `DelegateCommand`/`AsyncCommand`, the Fluent API (`SetBinding`, `BindCommand`, `BindCancelCommand`), predefined services (`IMessageBoxService`, `IDialogService`, …), behaviors, and `Messenger` ViewModel communication | [MVVM Framework Documentation](https://docs.devexpress.com/WindowsForms/113955/cross-platform-app-development/winforms-mvvm) |

---

## Skill Folder Content

Each skill is self-contained and follows the same structure:

```
devexpress-winforms-<name>/
├── SKILL.md      -- YAML frontmatter (activators, prerequisites), navigation guide
├── references/   -- scenario-focused deep dives
└── examples/     -- getting-started samples (C#)
```

---

For agent-specific and IDE-specific setup instructions, see the [repository README](../../README.md).
