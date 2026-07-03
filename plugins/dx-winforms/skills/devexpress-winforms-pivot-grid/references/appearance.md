# Appearance Customization — WinForms PivotGridControl (DevExpress v26.1)

## Overview

Three levels of appearance customization are available:

| Level | API | Best for |
|---|---|---|
| **Appearance properties** | `pivotGridControl1.Appearance.*` | Consistent palette for all cells of a type |
| **Per-field appearance** | `field.Appearance.*` | Style a specific field's header/values |
| **Custom Draw events** | `CustomDrawCell`, `CustomDrawFieldValue`, … | Pixel-level control over any element |

---

## PivotGridControl.Appearance — Global Settings

Access via `pivotGridControl1.Appearance` (returns `PivotGridAppearances`):

```csharp
// Data cells
pivotGridControl1.Appearance.Cell.BackColor  = Color.WhiteSmoke;
pivotGridControl1.Appearance.Cell.ForeColor  = Color.Black;
pivotGridControl1.Appearance.Cell.Font       = new Font("Segoe UI", 9f);

// Total cells (subtotals)
pivotGridControl1.Appearance.TotalCell.BackColor = Color.LightSteelBlue;
pivotGridControl1.Appearance.TotalCell.ForeColor = Color.DarkBlue;
pivotGridControl1.Appearance.TotalCell.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);

// Grand total cells
pivotGridControl1.Appearance.GrandTotalCell.BackColor = Color.SteelBlue;
pivotGridControl1.Appearance.GrandTotalCell.ForeColor = Color.White;

// Field headers
pivotGridControl1.Appearance.FieldHeader.BackColor = Color.DarkSlateBlue;
pivotGridControl1.Appearance.FieldHeader.ForeColor = Color.White;

// Focused / selected cells
pivotGridControl1.Appearance.FocusedCell.BackColor  = Color.Yellow;
pivotGridControl1.Appearance.SelectedCell.BackColor = Color.LightYellow;
```

### All Appearance Properties

| Property | Element |
|---|---|
| `Cell` | Regular data cells |
| `TotalCell` | Subtotal cells |
| `GrandTotalCell` | Grand-total cells |
| `FieldValue` | Field value labels (row/column headers) |
| `FieldValueTotal` | Field value labels for total rows/columns |
| `FieldValueGrandTotal` | Field value labels for grand-total rows/columns |
| `FieldHeader` | Field header boxes |
| `HeaderArea` | The entire header area background |
| `ColumnHeaderArea` | Column header area |
| `RowHeaderArea` | Row header area |
| `FilterHeaderArea` | Filter header area strip |
| `FilterPanel` | Filter Panel bar at the bottom |
| `FilterSeparator` | Separator between filter area and headers |
| `HeaderGroupLine` | Connector line for grouped fields |
| `FocusedCell` | Currently focused cell |
| `SelectedCell` | Selected cells (multi-select) |
| `SortByColumnIndicatorImage` | Glyph for "Sort by Summary" indicator |

---

## Per-Field Appearance

Each field has an `Appearance` property (`PivotGridFieldAppearances`) overriding the global settings:

```csharp
// Style the header of a specific field
fieldCategory.Appearance.Header.BackColor = Color.DarkOrange;
fieldCategory.Appearance.Header.ForeColor = Color.White;

// Style values displayed for this field (row/column labels)
fieldCategory.Appearance.Value.Font = new Font("Segoe UI", 10f, FontStyle.Italic);
```

Per-field appearance settings take precedence over `PivotGridControl.Appearance`.

> **Note**: Header background colors are **not effective** when WinForms Skins (XP/Office2003/Skin) are applied.

---

## Cell Value Format

Format the display text of data cells without affecting color:

```csharp
fieldSales.CellFormat.FormatType   = DevExpress.Utils.FormatType.Numeric;
fieldSales.CellFormat.FormatString = "c2";   // currency, 2 decimal places

fieldPercent.CellFormat.FormatType   = DevExpress.Utils.FormatType.Numeric;
fieldPercent.CellFormat.FormatString = "p1"; // percentage, 1 decimal place
```

---

## CustomAppearance Event (Cell-Level Colors Without Custom Draw)

The `CustomAppearance` event is raised for each cell and lets you modify its `AppearanceObject` without taking over the entire drawing process:

```csharp
pivotGridControl1.CustomAppearance += PivotGrid_CustomAppearance;

private void PivotGrid_CustomAppearance(object sender, PivotCustomAppearanceEventArgs e)
{
    // Highlight negative values in red
    if (e.DataField == fieldProfit && e.Value is double value && value < 0)
    {
        e.Appearance.BackColor = Color.MistyRose;
        e.Appearance.ForeColor = Color.DarkRed;
    }
}
```

`PivotCustomAppearanceEventArgs` key members:
- `e.DataField` — the data field this cell belongs to
- `e.Value` — cell summary value
- `e.Appearance` — `AppearanceObject` to modify
- `e.RowValueType` / `e.ColumnValueType` — `PivotGridValueType.Value/Total/GrandTotal`

---

## CustomDrawCell Event (Full Manual Painting)

Set `e.Handled = true` to suppress the default rendering and draw manually:

```csharp
pivotGridControl1.CustomDrawCell += PivotGrid_CustomDrawCell;

private void PivotGrid_CustomDrawCell(object sender, PivotCustomDrawCellEventArgs e)
{
    // Color grand-total cells differently
    if (e.RowValueType == PivotGridValueType.GrandTotal)
    {
        var fillBrush = e.GraphicsCache.GetSolidBrush(Color.SteelBlue);
        e.GraphicsCache.FillRectangle(fillBrush, e.Bounds);

        var textRect = e.Bounds;
        textRect.Inflate(-4, -4);
        e.GraphicsCache.DrawString(
            e.DisplayText,
            e.Appearance.Font,
            Brushes.White,
            textRect,
            e.Appearance.GetStringFormat());

        e.Handled = true;
        return;
    }

    // Normal cells — change background based on selection state
    Color bg = e.Focused  ? Color.White :
               e.Selected ? Color.LightCyan : Color.FromArgb(236, 251, 248);

    var brush = e.GraphicsCache.GetSolidBrush(bg);
    var r = e.Bounds;
    e.GraphicsCache.DrawRectangle(Pens.DimGray, r);
    r.Inflate(-1, -1);
    e.GraphicsCache.FillRectangle(brush, r);

    if (e.Focused)
    {
        r.Inflate(-1, -1);
        e.GraphicsCache.DrawRectangle(
            e.GraphicsCache.GetPen(Color.OrangeRed, 3), r);
    }

    r.Inflate(-4, -4);
    e.Appearance.DrawString(e.GraphicsCache, e.DisplayText, r);
    e.Handled = true;
}
```

### PivotCustomDrawCellEventArgs Key Members

| Member | Description |
|---|---|
| `e.Bounds` | Cell bounding rectangle |
| `e.DisplayText` | Formatted text to display |
| `e.Value` | Raw cell value |
| `e.Appearance` | Cell's `AppearanceObject` |
| `e.GraphicsCache` | DevExpress `GraphicsCache` for GDI+ drawing |
| `e.Focused` | Whether the cell is focused |
| `e.Selected` | Whether the cell is selected |
| `e.RowValueType` | `PivotGridValueType.Value/Total/GrandTotal` |
| `e.ColumnValueType` | Same for the column axis |
| `e.DataField` | Which data field this cell belongs to |
| `e.Handled` | Set `true` to suppress default drawing |

---

## Other Custom Draw Events

| Event | Element painted |
|---|---|
| `CustomDrawCell` | Data area cells |
| `CustomDrawFieldValue` | Row/column field values (headers) |
| `CustomDrawFieldHeader` | Field header boxes |
| `CustomDrawFieldHeaderArea` | Entire header area background |
| `CustomDrawEmptyArea` | Empty area (below fields in filter area) |

---

## Skins / Look and Feel

```csharp
using DevExpress.LookAndFeel;

// Apply a skin to the whole application
UserLookAndFeel.Default.SetSkinStyle("Office 2019 Colorful");

// Or per-control
pivotGridControl1.LookAndFeel.SkinName = "Visual Studio 2022 Dark";
```

When a skin is active, some `Appearance` properties (e.g. `BackColor` on field headers) are ignored.

---

## Key API Reference

| Member | Description |
|---|---|
| `PivotGridControl.Appearance` | `PivotGridAppearances` — global cell/header styles |
| `PivotGridAppearances.Cell` | Regular data cell appearance |
| `PivotGridAppearances.TotalCell` | Subtotal cell appearance |
| `PivotGridAppearances.GrandTotalCell` | Grand-total cell appearance |
| `PivotGridAppearances.FieldHeader` | Field header appearance |
| `PivotGridAppearances.FocusedCell` | Focused cell appearance |
| `PivotGridAppearances.SelectedCell` | Selected cell appearance |
| `PivotGridField.Appearance` | Per-field appearance overrides |
| `PivotGridField.CellFormat` | `FormatInfo` — format type and string |
| `PivotGridControl.CustomAppearance` | Event: modify cell appearance without full custom draw |
| `PivotGridControl.CustomDrawCell` | Event: fully custom-paint a data cell |
| `PivotGridControl.CustomDrawFieldValue` | Event: custom-paint field value labels |
| `PivotGridControl.CustomDrawFieldHeader` | Event: custom-paint field header boxes |
| `PivotGridControl.CustomDrawFieldHeaderArea` | Event: custom-paint header area background |
| `PivotGridControl.CustomDrawEmptyArea` | Event: custom-paint empty area |
| `PivotCustomDrawCellEventArgs.Handled` | Set `true` to suppress default rendering |
| `PivotCustomDrawCellEventArgs.GraphicsCache` | DevExpress drawing helper |
| `AppearanceObject.BackColor/ForeColor/Font` | Core style properties |
| `PivotGridControl.LookAndFeel.SkinName` | Per-control skin |
| `UserLookAndFeel.Default.SetSkinStyle(name)` | Application-wide skin |

---

## Source

- Appearances: https://docs.devexpress.com/content/WindowsForms/1816?md=true
- Custom Draw: https://docs.devexpress.com/content/WindowsForms/1817?md=true
- Elements Painted via Appearances: https://docs.devexpress.com/content/WindowsForms/9264?md=true
- Look and Feel: https://docs.devexpress.com/content/WindowsForms/1815?md=true
