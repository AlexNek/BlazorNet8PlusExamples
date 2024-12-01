namespace BlazorNet9RenderModes.Client.Model;

public class HierarchyRectangleModel
{
    public enum ERenderMode
    {
        Default = 0,

        InteractiveServer,

        InteractiveWebAssembly,

        InteractiveAuto
    }

    public string BackgroundColor { get; set; }

    public List<HierarchyRectangleModel> Children { get; set; }

    public string Text { get; set; }

    public ERenderMode? RenderMode { get; set; }
}
