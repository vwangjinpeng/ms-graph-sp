using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace pdf_editor
{
    public class PdfToDocxConverter
    {
        /// <summary>
        /// 使用 Microsoft Office Interop 将 PDF 转换为 DOCX
        /// </summary>
        /// <param name="pdfPath">PDF文件路径</param>
        /// <param name="docxPath">输出的DOCX文件路径</param>
        /// <param name="visible">是否显示Word应用程序</param>
        public static void ConvertUsingInterop(string pdfPath, string docxPath, bool visible = false)
        {
            if (!File.Exists(pdfPath))
                throw new FileNotFoundException("PDF文件不存在", pdfPath);

            Application wordApp = null;
            Document doc = null;

            try
            {
                // 创建Word应用程序实例
                wordApp = new Application { Visible = visible };

                // 设置不显示警告对话框
                wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;

                // 打开PDF文件
                doc = wordApp.Documents.Open(
                    FileName: pdfPath,
                    ConfirmConversions: false,
                    ReadOnly: true,
                    AddToRecentFiles: false,
                    Revert: false
                );

                // 转换为DOCX格式
                doc.SaveAs2(
                    FileName: docxPath,
                    FileFormat: WdSaveFormat.wdFormatDocumentDefault,
                    CompatibilityMode: WdCompatibilityMode.wdWord2013
                );

                Console.WriteLine($"转换成功: {docxPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"转换失败: {ex.Message}");
                throw;
            }
            finally
            {
                // 清理资源
                if (doc != null)
                {
                    doc.Close(false);
                    Marshal.ReleaseComObject(doc);
                }

                if (wordApp != null)
                {
                    wordApp.Quit(false);
                    Marshal.ReleaseComObject(wordApp);
                }

                // 强制垃圾回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
