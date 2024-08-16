using UnityEngine;
using UnityEngine.UI;
using Paroxe.PdfRenderer; // Assuming you're using PDFRenderer plugin
using NativeFilePickerNamespace; // Namespace for Native File Picker

public class PDFNativeFilePicker : MonoBehaviour
{
    public PDFViewer pdfViewer; // Reference to your PDFViewer prefab
    public Button browseButton; // Reference to your Browse Button

    void Start()
    {
        // Add listener to the Browse button
        browseButton.onClick.AddListener(OpenFileBrowser);
    }

    void OpenFileBrowser()
    {
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("Operation cancelled");
                return;
            }

            // Ensure the file has a .pdf extension
            if (path.EndsWith(".pdf"))
            {
                // Load the selected PDF file into the viewer
                pdfViewer.  LoadDocumentFromFile(path);
            }
            else
            {
                Debug.LogError("The selected file is not a PDF.");
            }
        }, new string[] { "application/pdf" });

        Debug.Log("Permission result: " + permission);
    }
}
