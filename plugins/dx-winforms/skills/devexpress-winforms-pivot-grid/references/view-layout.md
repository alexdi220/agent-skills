# View Layout — WinForms PivotGridControl (DevExpress v26.1)

## Field Areas

Every `PivotGridField` is placed into one of four areas via the `Area` and `AreaIndex` properties.

| `PivotArea` value | Visual location | Purpose |
|---|---|---|
| `RowArea` | Left edge | Row headers — values form row hierarchy |
| `ColumnArea` | Top edge | Column headers — values form column hierarchy |
| `DataArea` | Cells grid | Aggregated summary values |
| `FilterArea` | Top filter strip | Hidden from the grid; limits which records are included |

```csharp
field.Area      = PivotArea.RowArea;
field.AreaIndex = 0;   // position within the area (0-based)
```

Fields can be moved at runtime by the user via drag-and-drop (enabled by default).

---

## Moving Fields in Code

```csharp
// Move a field to a different area
fieldCategory.Area      = PivotArea.ColumnArea;
fieldCategory.AreaIndex = 0;

// Hide a field from the grid (still in Fields collection, moved to filter area)
fieldCountry.Area = PivotArea.FilterArea;
```

---

## Totals and Grand Totals Visibility

```csharp
// Hide all column totals
pivotGridControl1.OptionsView.ShowColumnTotals     = false;

// Hide all row totals
pivotGridControl1.OptionsView.ShowRowTotals        = false;

// Hide grand total row/column
pivotGridControl1.OptionsView.ShowColumnGrandTotals = false;
pivotGridControl1.OptionsView.ShowRowGrandTotals    = false;

// Move row totals to the left (default is right/far)
pivotGridControl1.OptionsView.RowTotalsLocation    = PivotRowTotalsLocation.Near;
// Move column totals to the top
pivotGridControl1.OptionsView.ColumnTotalsLocation = PivotTotalsLocation.Near;
```

### Compact / Dense Layout

```csharp
pivotGridControl1.OptionsView.ShowColumnTotals      = false;
pivotGridControl1.OptionsView.ShowRowTotals         = false;
pivotGridControl1.OptionsView.ShowColumnGrandTotals = false;
pivotGridControl1.OptionsView.ShowRowGrandTotals    = false;
pivotGridControl1.OptionsView.ShowRowHeaders        = false;
```

---

## Controlling Where Data Field Headers Appear

When there are multiple data fields, a special "Data" header is added to the column or row header area:

```csharp
// Show data-field headers in the row area instead of column area
pivotGridControl1.OptionsDataField.Area            = PivotDataArea.RowArea;
// Position of the data-field header strip among row fields
pivotGridControl1.OptionsDataField.AreaIndex       = 0;
```

---

## Expand and Collapse

```csharp
// Collapse / expand all rows
pivotGridControl1.CollapseAllRows();
pivotGridControl1.ExpandAllRows();

// Collapse / expand all columns
pivotGridControl1.CollapseAllColumns();
pivotGridControl1.ExpandAllColumns();

// Async equivalents
await pivotGridControl1.ExpandAllColumnsAsync();
await pivotGridControl1.CollapseAllRowsAsync();

// Expand a specific row value (isColumn = false; values identify the row)
pivotGridControl1.ExpandValue(false, new object[] { "Beverages" });

// Collapse a single column header value (isColumn = true; values identify the column)
pivotGridControl1.CollapseValue(true, new object[] { 2023 });
```

---

## Best Fit (Auto Column / Row Width)

```csharp
// Best-fit all columns and rows
pivotGridControl1.BestFit();

// Best-fit only the row-area columns
pivotGridControl1.BestFitRowArea();

// Best-fit only the column-area columns
pivotGridControl1.BestFitColumnArea();
```

Typically called in the `Load` event or after `EndUpdate()`.

---

## Field Groups

Use `PivotGridGroup` to group related fields that move together as a hierarchy:

```csharp
// Year + Quarter always move as a pair
PivotGridGroup dateGroup = new PivotGridGroup();
dateGroup.AddRange(new PivotGridField[] { fieldYear, fieldQuarter });
pivotGridControl1.Groups.Add(dateGroup);
```

- All fields in a group must be in the same area.
- Moving one field moves the whole group.
- The group is shown with a connecting bar in the header area.
- Access groups via `pivotGridControl1.Groups`.

---

## Customization Form

The built-in customization form lets users drag fields between areas at runtime:

```csharp
// Open programmatically
pivotGridControl1.ShowCustomization();

// Control visibility with OptionsCustomization
pivotGridControl1.OptionsCustomization.AllowDrag             = true;
pivotGridControl1.OptionsCustomization.AllowSort             = true;
pivotGridControl1.OptionsCustomization.AllowFilter           = true;
pivotGridControl1.OptionsCustomization.AllowExpand           = true;
```

---

## Save and Restore Layout

```csharp
// Save to XML
pivotGridControl1.SaveLayoutToXml("layout.xml");

// Restore from XML
pivotGridControl1.RestoreLayoutFromXml("layout.xml");

// Save to stream
using var ms = new MemoryStream();
pivotGridControl1.SaveLayoutToStream(ms);

// Restore from stream
ms.Position = 0;
pivotGridControl1.RestoreLayoutFromStream(ms);
```

---

## Data Cell Layout

By default, each cell shows one summary value. When multiple data fields exist the grid can stack them:

```csharp
// Display multiple data-field values per cell row
pivotGridControl1.OptionsDataField.Area      = PivotDataArea.ColumnArea;
pivotGridControl1.OptionsDataField.AreaIndex = 1;
```

For compact/single-column drill-down, place the Data header in the row area at `AreaIndex = 0`.

---

## Pinned (Fixed) Columns

```csharp
// Fix the first n column-area cells from horizontal scrolling
pivotGridControl1.OptionsLayout.ColumnFixedWidth = 200; // pixels
```

---

## Key API Reference

| Member | Description |
|---|---|
| `PivotGridField.Area` | `PivotArea` enum — target area |
| `PivotGridField.AreaIndex` | Position within the area |
| `OptionsView.ShowColumnTotals` / `ShowRowTotals` | Show/hide subtotals |
| `OptionsView.ShowColumnGrandTotals` / `ShowRowGrandTotals` | Show/hide grand totals |
| `OptionsView.RowTotalsLocation` | `PivotRowTotalsLocation.Near` / `Far` |
| `OptionsView.ColumnTotalsLocation` | `PivotTotalsLocation.Near` / `Far` |
| `OptionsDataField.Area` / `AreaIndex` | Data-header strip placement |
| `CollapseAllRows()` / `ExpandAllRows()` | Expand/collapse rows |
| `CollapseAllColumns()` / `ExpandAllColumns()` | Expand/collapse columns |
| `BestFit()` / `BestFitRowArea()` / `BestFitColumnArea()` | Auto-size columns |
| `PivotGridGroup` | Field grouping |
| `Groups.Add(group)` | Register a group |
| `ShowCustomization()` | Open the customization form |
| `SaveLayoutToXml(path)` / `RestoreLayoutFromXml(path)` | Layout persistence |
| `OptionsCustomization.AllowDrag` | Allow end-user drag-and-drop |

---

## Source

- Field Groups: https://docs.devexpress.com/content/WindowsForms/1958?md=true
- Best Fit: https://docs.devexpress.com/content/WindowsForms/14611?md=true
- Customization Form: https://docs.devexpress.com/content/WindowsForms/1805?md=true
- Save and Restore Layout: https://docs.devexpress.com/content/WindowsForms/1806?md=true
- Data Cell Layout: https://docs.devexpress.com/content/WindowsForms/1804?md=true
