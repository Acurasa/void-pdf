using VoidPDF.Data;
using VoidPDF.Data.Helpers;

namespace VoidPDF.Core;
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
public static class PdfSanitizer
{
    public static async Task<byte[]> SanitizeAsync(byte[] pdfBytes, Metadata? metadata = null, QuickSanitizeOptions? options = null)
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

            if (options == QuickSanitizeOptions.RemoveMetadata || options == QuickSanitizeOptions.FullClean)
            {
                info.SetTitle("");
                info.SetAuthor("");
                info.SetSubject("");
                info.SetCreator("");
                info.SetProducer("");
            }

            if (options == QuickSanitizeOptions.RemoveKeywords || options == QuickSanitizeOptions.FullClean)
                info.SetKeywords("");

            if (metadata is not null)
            {
                info.SetTitle(metadata.Title ?? info.GetTitle());
                info.SetAuthor(metadata.Author ?? info.GetAuthor());
                info.SetSubject(metadata.Subject ?? info.GetSubject());
                info.SetCreator(metadata.Creator ?? info.GetCreator());
                info.SetProducer(metadata.Producer ?? info.GetProducer());
                info.SetKeywords(metadata.Keywords ?? info.GetKeywords());
            }

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
}