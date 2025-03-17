using iText.Forms;
using VoidPDF.Data;
using VoidPDF.Data.Helpers;

namespace VoidPDF.Core;
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
public static class PdfSanitizer
{
   public static async Task<byte[]> SanitizeAsync(byte[] pdfBytes, Metadata? metadata, SanitizeOptions options)
{
    if (pdfBytes == null || pdfBytes.Length == 0)
        throw new ArgumentException("Pdf bytes cannot be null or empty.");

    try
    {
        await using var inputStream = new MemoryStream(pdfBytes);
        await using var outputStream = new MemoryStream();

        using var reader = new PdfReader(inputStream);
        using var writer = new PdfWriter(outputStream);
        using var document = new PdfDocument(reader, writer);

        PdfDocumentInfo info = document.GetDocumentInfo();

        if (options.FullClean || options.ClearMetadata)
        {
            info.SetTitle("");
            info.SetAuthor("");
            info.SetSubject("");
            info.SetCreator("");
            info.SetProducer("");
            info.SetKeywords("");
        }

        if (metadata is not null)
        {
            info.SetTitle(metadata.Title ?? info.GetTitle());
            info.SetAuthor(metadata.Author ?? info.GetAuthor());
            info.SetSubject(metadata.Subject ?? info.GetSubject());
            info.SetCreator(metadata.Creator ?? info.GetCreator());
            info.SetProducer(metadata.Producer ?? info.GetProducer());
            info.SetKeywords(metadata.Keywords ?? info.GetKeywords());
        }

        if (options.FullClean || options.DeleteJavaScript)
            RemoveJavaScript(document);

        if (options.FullClean || options.RemoveAnnotations)
            RemoveAnnotations(document);

        if (options.FullClean || options.RemoveFormFields)
            RemoveFormFields(document);

        if (options.FullClean || options.RemoveAcroForms)
            RemoveAcroForms(document);

        if (options.FullClean || options.RemoveActions)
            RemoveActions(document);

        if (options.FullClean || options.RemoveEmbeddedFiles)
            RemoveEmbeddedFiles(document);

        document.Close();
        return outputStream.ToArray();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERR: {ex.Message}");
        throw;
    }
}





    public static Metadata GetActualMetaData(PdfDocumentInfo info)
    {
        return new Metadata
        {
            Title = info.GetTitle(),
            Subject = info.GetSubject(),
            Author = info.GetAuthor(),
            Producer = info.GetProducer(),
            Creator = info.GetCreator(),
            Keywords = info.GetKeywords(),
        };
    }
    
    private static void RemoveJavaScript(PdfDocument document)
    {
        var catalog = document.GetCatalog();
        catalog.Remove(PdfName.JS);
    }


    private static void RemoveAnnotations(PdfDocument document)
    {
        for (int i = 1; i <= document.GetNumberOfPages(); i++)
        {
            var page = document.GetPage(i);
            var annotations = page.GetAnnotations();
        
            foreach (var annotation in annotations)
            {
                page.RemoveAnnotation(annotation);
            }
        }
    }


    private static void RemoveAcroForms(PdfDocument document)
    {
        var form = PdfAcroForm.GetAcroForm(document, false);
        form?.FlattenFields();
    }

    private static void RemoveFormFields(PdfDocument document)
    {
        var form = PdfAcroForm.GetAcroForm(document, false);
        if (form != null)
        {
            var fields = form.GetAllFormFields();
            foreach (var fieldName in fields.Keys.ToList()) 
            {
                form.RemoveField(fieldName);
            }
        }
    }




    private static void RemoveEmbeddedFiles(PdfDocument document)
    {
        var names = document.GetCatalog().GetPdfObject().GetAsDictionary(PdfName.Names);
        if (names != null)
        {
            names.Remove(PdfName.EmbeddedFiles);
        }
    }

    //  (Launch, URI, и т. д.)
    private static void RemoveActions(PdfDocument document)
    {
        var catalog = document.GetCatalog();
        catalog.Remove(PdfName.OpenAction);
        catalog.Remove(PdfName.AA);
    }
    
 
}