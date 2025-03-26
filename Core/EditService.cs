using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;

namespace YourBlazorApp.Services
{
    public class EditService
    {
        public MemoryStream HighlightText(Stream pdfStream, double x, double y, double width, double height, int pageIndex)
        {
            // Открываем PDF документ из потока
            PdfDocument document = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify);
            
            // Получаем страницу, на которой нужно выделить текст
            if (pageIndex >= 0 && pageIndex < document.Pages.Count)
            {
                PdfPage page = document.Pages[pageIndex];
                
                // Создаем графический контекст для страницы
                XGraphics gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);
                
                // Создаем желтую кисть для выделения
                XSolidBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 255, 0));
                
                // Рисуем прямоугольник для выделения текста
                gfx.DrawRectangle(brush, x, y, width, height);
            }
            
            // Сохраняем изменения в новый поток
            MemoryStream outputStream = new MemoryStream();
            document.Save(outputStream, false);
            outputStream.Position = 0;
            
            return outputStream;
        }
        
        public MemoryStream LoadPdf(string filePath)
        {
            // Проверяем существование файла
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"PDF файл не найден: {filePath}");
            }
            
            // Читаем файл в память
            byte[] fileBytes = File.ReadAllBytes(filePath);
            MemoryStream stream = new MemoryStream(fileBytes);
            
            return stream;
        }
    }
}