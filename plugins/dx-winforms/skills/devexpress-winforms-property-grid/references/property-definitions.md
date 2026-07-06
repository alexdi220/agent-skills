# Property Definitions — Row Configuration

The Property Grid exposes properties through **rows**. Each row is either auto-generated (from reflection when `SelectedObject` is set) or manually created in code or the designer. This reference covers configuring rows and their appearance.

## When to Use This Reference

- Configuring an `EditorRow` (`FieldName`, `Caption`, `RowEdit`, `Format`, `ReadOnly`, `Visible`)
- Creating a `MultiEditorRow` for side-by-side editors (Classic view only)
- Mapping a type to a default in-place editor globally via `DefaultEditors`
- Filtering which properties appear with `CustomPropertyDescriptors`
- Customizing auto-generated rows in `CustomRowCreated`
- Accessing rows by `GetRowByFieldName` / `GetRowByCaption`

---

## Row Types

| Type | Purpose |
|---|---|
| `EditorRow` | Single property bound to a field (`FieldName`) |
| `CategoryRow` | Header row that groups child rows; no editor |
| `MultiEditorRow` | Multiple property editors side by side in one row (Classic view only) |
| `PGridNumericEditorRow` | EditorRow with optional track bar (Office view) |
| `PGridCustomEditorRow` | EditorRow with a fully custom control |

---

## Configuring an EditorRow

Each row's display and editing is controlled through `row.Properties`:

```csharp
// Access auto-generated rows by field name
var priceRow = propertyGridControl1.GetRowByFieldName("Price");

// Caption and format
priceRow.Properties.Caption              = "Unit Price";
priceRow.Properties.Format.FormatType    = DevExpress.Utils.FormatType.Numeric;
priceRow.Properties.Format.FormatString  = "C2";  // currency format

// Assign a specific in-place editor
priceRow.Properties.RowEdit = new RepositoryItemSpinEdit {
    MinValue = 0,
    MaxValue = 9999
};

// Make a row read-only
priceRow.Properties.ReadOnly = true;

// Hide a row
priceRow.Visible = false;
```

`GetRowByFieldName` requires rows to already exist (either auto-generated after `SelectedObject` is set, or manually created).

---

## Creating Rows Manually

If you define rows in code before setting `SelectedObject`, the control uses your rows rather than generating its own.

```csharp
// EditorRow bound to a property
var nameRow = new EditorRow();
nameRow.Properties.FieldName = "Name";
nameRow.Properties.Caption   = "Full Name";
propertyGridControl1.Rows.Add(nameRow);

// Row with a custom editor
var colorRow = new EditorRow();
colorRow.Properties.FieldName = "BackColor";
colorRow.Properties.Caption   = "Background Color";
colorRow.Properties.RowEdit   = new RepositoryItemColorEdit();
propertyGridControl1.Rows.Add(colorRow);

propertyGridControl1.SelectedObject = mySettings;
```

---

## MultiEditorRow — Multiple Editors in One Row

Puts multiple property editors side-by-side in a single row. Classic view only.

```csharp
var meRow = new MultiEditorRow();
meRow.PropertiesCollection.Add(new MultiEditorRowProperties {
    FieldName = "FirstName", Caption = "First"
});
meRow.PropertiesCollection.Add(new MultiEditorRowProperties {
    FieldName = "LastName", Caption = "Last"
});
propertyGridControl1.Rows.Add(meRow);
```

---

## Default Editors — Associate a Type with an Editor

Override the editor used for all properties of a given type across the entire control:

```csharp
// Use CheckEdit instead of ComboBox for all bool properties
var riCheck = new RepositoryItemCheckEdit();
propertyGridControl1.DefaultEditors.Add(typeof(bool), riCheck);

// Custom spin edit for all int properties
var riSpin = new RepositoryItemSpinEdit { MinValue = 0, MaxValue = int.MaxValue };
propertyGridControl1.DefaultEditors.Add(typeof(int), riSpin);
```

---

## Filtering Properties at Runtime

Handle `CustomPropertyDescriptors` to control which properties appear. Called once for root properties and once per expanded nested object.

```csharp
// Show only a subset of root properties
void propertyGridControl1_CustomPropertyDescriptors(
        object sender, CustomPropertyDescriptorsEventArgs e) {
    // e.Context.PropertyDescriptor == null → root level
    if (e.Context.PropertyDescriptor == null) {
        var filtered = new PropertyDescriptorCollection(null);
        foreach (string name in new[] { "Title", "MaxThreads", "StartMinimized" })
            if (e.Properties[name] != null)
                filtered.Add(e.Properties[name]);
        e.Properties = filtered;
    }
}
```

To exclude a single property:

```csharp
e.Properties = new PropertyDescriptorCollection(
    e.Properties.OfType<PropertyDescriptor>()
        .Where(p => p.Name != "InternalId")
        .ToArray());
```

---

## Customizing Rows Dynamically — CustomRowCreated

Fires for each auto-generated row just before it appears. Use it to customize rows based on the field name or property type.

```csharp
void propertyGridControl1_CustomRowCreated(
        object sender, CustomRowCreatedEventArgs e) {
    // Replace numeric row with trackbar variant (Office view only)
    if (e.Row is PGridNumericEditorRow numRow &&
        e.Row.Properties.FieldName == "Opacity") {
        numRow.ShowTrackBar = true;
        numRow.MinValue     = 0;
        numRow.MaxValue     = 100;
    }

    // Hide a specific auto-generated row
    if (e.Row.Properties.FieldName == "InternalDebugInfo") {
        e.Row = new PGridEmptyRow();
        e.Handled = true;
    }
}
```

---

## Accessing Rows

```csharp
// By bound field name (most common)
var row = propertyGridControl1.GetRowByFieldName("MaxThreads");

// By display caption
var row2 = propertyGridControl1.GetRowByCaption("Maximum Threads");

// By row handle index
var row3 = propertyGridControl1.GetRowByHandle(0);

// Enumerate all root rows
foreach (BaseRow row in propertyGridControl1.Rows) { ... }
```

---

## Refresh When Object Properties Change

If the bound object changes its property values, force the grid to repaint:

```csharp
// Reload property values from the bound object
propertyGridControl1.RefreshAllProperties();

// Repaint without reloading values
propertyGridControl1.RepaintAllProperties();
```

---

## Source Material

- Rows reference: `https://docs.devexpress.com/content/WindowsForms/401851?md=true`
- Classic View: `https://docs.devexpress.com/content/WindowsForms/401865?md=true`
- `PropertyGridControl` class: `https://docs.devexpress.com/content/WindowsForms/DevExpress.XtraVerticalGrid.PropertyGridControl?md=true`
