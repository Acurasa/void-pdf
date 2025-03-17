namespace VoidPDF.Data;

public class SanitizeOptions
{
    public bool FullClean { get; set; }
    public bool DeleteJavaScript { get; set; }
    public bool RemoveAnnotations { get; set; }
    public bool RemoveFormFields { get; set; }
    public bool RemoveAcroForms { get; set; }
    public bool RemoveActions { get; set; }
    public bool RemoveEmbeddedFiles { get; set; }
    public bool ClearMetadata { get; set; }
}
