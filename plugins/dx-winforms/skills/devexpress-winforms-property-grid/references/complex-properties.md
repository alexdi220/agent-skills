# Expanding Complex (Nested Object) Properties

When a property's type is a class (not a primitive), the Property Grid can display it as an **expandable row** — clicking the expand button reveals the sub-object's own properties as child rows.

## When to Use This Reference

- Making a nested object property expandable (`[TypeConverter(typeof(ExpandableObjectConverter))]` on the nested type)
- Accessing nested rows by their dotted `FieldName` path
- Expanding nested rows programmatically
- Filtering which nested sub-properties appear via `CustomPropertyDescriptors`
- Handling multiple levels of nesting

---

## Enabling Expansion: ExpandableObjectConverter

To make a complex property expandable, its **type** must use an `ExpandableObjectConverter`. Apply the `[TypeConverter]` attribute on the property's class declaration:

```csharp
using System.ComponentModel;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class DatabaseSettings {
    [DisplayName("Host")]
    public string Host { get; set; } = "localhost";

    [DisplayName("Port")]
    public int Port { get; set; } = 5432;

    [DisplayName("Database Name")]
    public string DatabaseName { get; set; } = "mydb";

    // Controls the text shown in the parent row's value cell
    public override string ToString() => $"{Host}:{Port}/{DatabaseName}";
}
```

Now any property of type `DatabaseSettings` appears as an expandable row in the Property Grid:

```csharp
public class AppSettings {
    [Category("Data")]
    [DisplayName("Database")]
    public DatabaseSettings Database { get; set; } = new DatabaseSettings();
}

propertyGridControl1.SelectedObject = new AppSettings();
```

The user sees a "Database" row that can be expanded to show Host, Port, and DatabaseName sub-rows. The collapsed row displays the value returned by `ToString()`.

---

## Expanding Rows Programmatically

After `SelectedObject` is assigned, access the row and set `Expanded`:

```csharp
void propertyGridControl1_DataSourceChanged(object sender, EventArgs e) {
    propertyGridControl1.GetRowByFieldName("Database").Expanded = true;
}
```

Access nested rows by dotted path:

```csharp
// Access the nested "Port" property inside "Database"
var portRow = propertyGridControl1.GetRowByFieldName("Database.Port");
portRow.Properties.Caption = "DB Port";
```

---

## Filtering Nested Properties

`CustomPropertyDescriptors` fires once for root properties and once for each expanded nested object. Use `e.Context.PropertyDescriptor` to determine which level is being filtered:

```csharp
void propertyGridControl1_CustomPropertyDescriptors(
        object sender, CustomPropertyDescriptorsEventArgs e) {
    // Root level: e.Context.PropertyDescriptor == null
    if (e.Context.PropertyDescriptor == null)
        return; // leave root unchanged

    // Nested level: filter the Database sub-properties
    if (e.Context.PropertyDescriptor.Name == "Database") {
        var filtered = new PropertyDescriptorCollection(null);
        foreach (string name in new[] { "Host", "Port" })
            if (e.Properties[name] != null)
                filtered.Add(e.Properties[name]);
        e.Properties = filtered;   // only show Host and Port
    }
}
```

---

## Multiple Levels of Nesting

Each nested type must also carry `[TypeConverter(typeof(ExpandableObjectConverter))]`:

```csharp
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ReplicaConfig {
    public string Host { get; set; } = "replica-host";
    public int Port { get; set; } = 5433;
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public class DatabaseSettings {
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;

    [DisplayName("Replica")]
    public ReplicaConfig Replica { get; set; } = new ReplicaConfig();

    public override string ToString() => $"{Host}:{Port}";
}
```

---

## Read-Only Complex Properties

If the sub-object should be viewable but not replaceable, use a read-only property. The sub-properties can still be editable if they have setters:

```csharp
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ProxySettings {
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; } = 8080;
}

public class NetworkSettings {
    // Can view/edit Proxy.Address and Proxy.Port, but cannot replace Proxy itself
    [Browsable(true)]
    public ProxySettings Proxy { get; } = new ProxySettings();
}
```

---

## Hiding the Expand Button for a Specific Property

Use `CustomRowCreated` to replace the row with an empty non-expandable one:

```csharp
void propertyGridControl1_CustomRowCreated(
        object sender, CustomRowCreatedEventArgs e) {
    // Show nested properties without an editor on the parent row
    if (e.Row.Properties.FieldName == "Database") {
        e.Row = new PGridEmptyRow();
        e.Handled = true;
    }
}
```

This hides the Database row header entirely while still showing its child rows if they are added via a `CategoryRow` in code.

---

## Summary: What Enables Expansion

| Requirement | Where to apply |
|---|---|
| `[TypeConverter(typeof(ExpandableObjectConverter))]` | On the **property's type class** (mandatory) |
| `ToString()` override | On the type class (controls the collapsed value display) |
| `GetRowByFieldName("PropName").Expanded = true` | In code, after `SelectedObject` is set |
| `CustomPropertyDescriptors` | To filter which nested properties appear |

---

## Source Material

- Rows: `https://docs.devexpress.com/content/WindowsForms/401851?md=true`
- Filter Properties example: `https://docs.devexpress.com/content/WindowsForms/8369?md=true`
- Office View (CustomRowCreated): `https://docs.devexpress.com/content/WindowsForms/120262?md=true`
- `PropertyGridControl` class: `https://docs.devexpress.com/content/WindowsForms/DevExpress.XtraVerticalGrid.PropertyGridControl?md=true`
