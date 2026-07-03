# AccordionControlElement Settings and Customization

`AccordionControlElement` represents both groups and items in the accordion hierarchy. Groups are collapsible containers; items are clickable leaves that can optionally embed arbitrary WinForms controls. This reference covers the full API for configuring and customizing individual elements.

## When to Use This Reference

- Setting up groups and items (text, icons, shortcuts)
- Embedding custom controls inside an item (content containers)
- Rearranging header content blocks (icon / text / button order)
- Customizing appearance per element
- Using HTML-CSS templates for rich custom layouts

## Group vs Item — Core Distinction

Both element kinds are `AccordionControlElement` objects. The `Style` property determines behavior:

| `ElementStyle` | Can contain children | Has `ContentContainer` | Raises `Click` |
|---|---|---|---|
| `Group` | Yes (`Elements` collection) | No | No (only expand/collapse) |
| `Item` | No (ignored) | Yes | Yes |

```csharp
var group = new AccordionControlElement(ElementStyle.Group);
var item  = new AccordionControlElement(ElementStyle.Item);
```

## Common Element Properties

| Property | Type | Description |
|---|---|---|
| `Text` | `string` | Header caption |
| `Name` | `string` | Designer/code identifier |
| `Tag` | `object` | User-defined payload (ID, route, view model) |
| `Enabled` | `bool` | Grays out the element when `false` |
| `Visible` | `bool` | Shows or hides the element |
| `Expanded` | `bool` | Initial expand state (groups and items with containers) |
| `HeaderVisible` | `bool` | Hides the header; only the content container is shown |
| `ShortcutKey` | `Keys` | Keyboard shortcut that fires the `Click` event |
| `Style` | `ElementStyle` | `Group` or `Item` |

## Images

Assign icons using `ImageOptions.ImageUri` (SVG from a DevExpress catalog) or `Image`/`ImageIndex` (classic bitmap):

```csharp
// SVG icon from the built-in DX catalog
item.ImageOptions.ImageUri.Uri = "Home;Office2013";

// Bitmap from an ImageList on the form
item.ImageIndex = 2;
accordion.ImageList = imageList1;

// Direct bitmap
item.Image = Image.FromFile("icons/settings.png");
```

## Content Containers (Items Only)

A content container (`AccordionContentContainer`) lets an item host arbitrary WinForms controls in an expandable area below its header.

### Assign at Design Time

In the item's smart tag, click **"Add ContentContainer"**, then drag controls from the Toolbox onto the container.

### Assign in Code

```csharp
var container = new AccordionContentContainer();
container.Controls.Add(new ToggleSwitch {
    Location = new Point(10, 8),
    Properties = { OnText = "Online", OffText = "Offline" }
});
container.Height = 50;
// Use skin-aware padding
container.Padding = new Padding(-1);

// Attach
mySettingsItem.ContentContainer = container;
accordion.Controls.Add(container);   // required — container must be a child of the accordion
```

### Lazy Loading via Events

For large menus, defer container creation until the item is first expanded:

```csharp
// Which items should get a container?
private void Accordion_HasContentContainer(object sender, HasContentContainerEventArgs e) {
    e.HasContentContainer = (e.Element.Tag as string) == "settings";
}

// Supply the container on demand
private void Accordion_GetContentContainer(object sender, GetContentContainerEventArgs e) {
    if ((e.Element.Tag as string) == "settings") {
        var uc = new SettingsUserControl { Dock = DockStyle.Fill };
        var container = new AccordionContentContainer();
        container.Controls.Add(uc);
        e.ContentContainer = container;
    }
}
```

> Each item can have at most one `ContentContainer`. Groups cannot have content containers.
>
> **Do NOT wrap a content container in `BeginInit()`/`EndInit()`.** `AccordionContentContainer` does **not** implement `ISupportInitialize`, so `((System.ComponentModel.ISupportInitialize)(container)).BeginInit();` throws `InvalidCastException: Unable to cast object of type 'DevExpress.XtraBars.Navigation.AccordionContentContainer' to type 'System.ComponentModel.ISupportInitialize'`. In a `*.Designer.cs`, only the `AccordionControl` itself is wrapped in `BeginInit`/`EndInit` — its content containers are added to `Controls` and assigned to `item.ContentContainer` like ordinary panels, without `BeginInit`/`EndInit`.

## Header Layout Customization

By default, each element header shows: **[icon] [text] [header control] [context buttons]** from left to right. You can rearrange these blocks per element.

### Rearranging in Code

Use `HeaderTemplate.Set*Position` methods:

```csharp
// Image → right; text → right; header control → left; context buttons → right
var tpl = accordionControlElement1.HeaderTemplate;
tpl.SetHeaderControlPosition(0, HeaderElementAlignment.Left);
tpl.SetTextPosition(1,          HeaderElementAlignment.Right);
tpl.SetImagePosition(2,         HeaderElementAlignment.Right);
tpl.SetContextButtonsPosition(3,HeaderElementAlignment.Right);
```

The numeric argument is the position index within the header (0 = leftmost).

### Spacing Between Blocks

```csharp
accordionControlElement1.HeaderIndent = 8;   // pixels between neighboring blocks
```

### Embedding a Custom Control in the Header

```csharp
var badge = new LabelControl { Text = "3", BackColor = Color.Red };
item.HeaderControl = badge;
```

## Selection

```csharp
// Enable visual selection highlighting when items are clicked
accordion.AllowItemSelection = true;

// Programmatically select an item
accordion.SelectedElement = myItem;
```

## Context Buttons

Context buttons appear on hover inside each element header. They are defined at the control level and can be customized per element:

```csharp
// Add a "delete" context button to a specific element via its own
// ContextButtons collection (AccordionControlElementBase.ContextButtons)
var deleteBtn = new AccordionContextButton { Name = "Delete", Hint = "Remove" };
element.ContextButtons.Add(deleteBtn);

// Read-only elements simply omit the button (do not add it / clear the collection)
if (readOnlyElement.Tag as string == "readOnly")
    readOnlyElement.ContextButtons.Clear();
```

> Context buttons are managed per element through `AccordionControlElementBase.ContextButtons`. The `ContextButtonCustomize` event args (`AccordionControlContextButtonCustomizeEventArgs`) expose only `Element` and `ContextItem` — there is no `Buttons` collection on the event args.

## Appearance

Override colors and fonts per element:

```csharp
item.Appearance.Normal.BackColor = Color.FromArgb(240, 248, 255);
item.Appearance.Normal.Font     = new Font("Segoe UI", 10, FontStyle.Bold);
item.Appearance.Hovered.BackColor = Color.LightSteelBlue;
```

Control-level defaults apply when per-element settings are not set:

```csharp
accordion.Appearance.ItemNormal.BackColor = Color.WhiteSmoke;
```

## HTML-CSS Templates

For pixel-perfect custom layouts, apply HTML-CSS templates through `AccordionControl.HtmlTemplates`:

```csharp
// Template for all items (HTML string)
accordion.HtmlTemplates.Item = new HtmlTemplate {
    Template = @"
        <div class='item'>
            <img class='icon' src='${Image}'>
            <div class='label'>${Text}</div>
        </div>",
    Styles = ".item { display:flex; align-items:center; gap:8px; }"
};
```

- `${Text}` — element caption
- `${Image}` — element icon (use inside an `<img src='${Image}'>`)
- `${MyField}` — custom data resolved in `QueryHtmlElementData`

```csharp
accordion.QueryHtmlElementData += (s, e) => {
    if (e.FieldName == "Badge") e.Value = GetBadgeCount(e.Element.Tag);
};
```

Per-element templates override control-level defaults. Handle `QueryItemTemplate` to supply different templates dynamically:

```csharp
accordion.QueryItemTemplate += (s, e) => {
    if (e.Element.Style == ElementStyle.Group)
        e.Template = groupTemplate;
};
```

## Custom Drawing

For full owner-draw control, handle `CustomDrawElement`:

```csharp
private void Accordion_CustomDrawElement(object sender, CustomDrawElementEventArgs e) {
    if (e.Element.Text == "Danger") {
        e.DrawImage();
        // Draw text in red
        using var brush = new SolidBrush(Color.DarkRed);
        e.Graphics.DrawString(e.Element.Text, e.Font, brush, e.TextBounds);
        // Skip default expand button for this element
        e.Handled = true;
    }
}
```

Available draw methods: `DrawImage()`, `DrawText()`, `DrawContextButtons()`, `DrawExpandCollapseButton()`.

## Key API Summary

| API | Description |
|---|---|
| `AccordionControlElement.Style` | `Group` or `Item` |
| `AccordionControlElement.Text` | Caption |
| `AccordionControlElement.Hint` | Tooltip text for the element (there is **no** `ToolTipText` property) |
| `AccordionControlElement.Tag` | User payload |
| `AccordionControlElement.Enabled` / `Visible` | Enable / show control |
| `AccordionControlElement.Expanded` | Initial expand state |
| `AccordionControlElement.ContentContainer` | Container for embedded controls (items only) |
| `AccordionControlElement.HeaderControl` | Custom control in the header |
| `AccordionControlElement.HeaderTemplate` | Per-element header block order |
| `AccordionControlElement.Appearance` | Per-element visual styles |
| `AccordionControlElement.ShortcutKey` | Keyboard shortcut |
| `AccordionControl.AllowItemSelection` | Enable item selection highlight |
| `AccordionControl.SelectedElement` | Get/set selected item |
| `AccordionControl.HtmlTemplates` | HTML-CSS templates for all element types |
| `AccordionControl.CustomDrawElement` | Owner-draw event |

## Common Issues

| Symptom | Cause | Solution |
|---|---|---|
| Content container not showing | Container not added to `accordion.Controls` | Call `accordion.Controls.Add(container)` after creating it |
| `ToolTipText` does not compile | No such property on `AccordionControlElement` | Set the element's `Hint` property for a tooltip |
| `InvalidCastException` casting `AccordionContentContainer` to `ISupportInitialize` | Content container wrapped in `BeginInit()`/`EndInit()` | Remove those calls — only the `AccordionControl` is wrapped in the designer; containers are not |
| `HasContentContainer` event not firing | Item already has a container assigned | The event fires only when `ContentContainer` is null |
| Custom header control overlaps text | Header layout not adjusted | Use `HeaderTemplate.Set*Position` to rearrange blocks |
| Image not appearing | `ImageList` not assigned to accordion | Set `accordion.ImageList` or use `ImageOptions.ImageUri` directly |
| Per-element appearance ignored | Skin overrides color | Check `Appearance.Options.UseBackColor = true` |

## Source Material

- `articles/114553` — Accordion Control overview (Items/Groups, Content Containers, Header Layout sections)
- `articles/115716` — Element Header Layout
- `articles/DevExpress.XtraBars.Navigation.AccordionControlElement` — AccordionControlElement class reference
- `articles/DevExpress.XtraBars.Navigation.AccordionControlElementBase` — AccordionControlElementBase class reference
- `articles/DevExpress.XtraBars.Navigation.AccordionControlHtmlTemplates` — HtmlTemplates property reference
