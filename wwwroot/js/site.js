function downloadFile(fileName, fileBase64) {
    var link = document.createElement("a");
    link.href = "data:application/pdf;base64," + fileBase64;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}