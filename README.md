# PdfSanitizer

PdfSanitizer is a Blazor Server application that provides functionality to sanitize and modify PDF metadata and contents using iText7.

## Features
- Remove or modify PDF metadata (title, author, subject, keywords, etc.).
- Remove JavaScript, annotations, form fields, and embedded files.
- Remove AcroForms and actions from PDFs.
- Full cleanup mode to apply all available sanitization options.

## Technologies Used
- **Blazor Server** for the frontend.
- **.NET** with C# as the backend.
- **iText7** for PDF processing.

## Installation
### Prerequisites
- .NET SDK 7.0+
- Blazor Server environment setup

### Clone the Repository
```sh
git clone https://github.com/yourusername/PdfSanitizer.git
cd PdfSanitizer
```

### Run the Application
```sh
dotnet restore
dotnet run
```

## Usage
### Sanitizing a PDF
Use the `PdfSanitizer.SanitizeAsync` method to clean a PDF file:
```csharp
byte[] sanitizedPdf = await PdfSanitizer.SanitizeAsync(pdfBytes, metadata, options);
```

### Options
```csharp
var options = new SanitizeOptions
{
    FullClean = true,
    ClearMetadata = true,
    DeleteJavaScript = true,
    RemoveAnnotations = true,
    RemoveFormFields = true,
    RemoveAcroForms = true,
    RemoveActions = true,
    RemoveEmbeddedFiles = true
};
```

### Metadata Example
```csharp
var metadata = new Metadata
{
    Title = "My Clean PDF",
    Author = "John Doe",
    Subject = "Sanitized Document",
    Keywords = "clean, safe, pdf"
};
```

## Contributing
Feel free to submit issues and pull requests to improve the project.

## License
MIT License

