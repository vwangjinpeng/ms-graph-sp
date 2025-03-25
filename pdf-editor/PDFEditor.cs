using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pdf_editor
{
    public partial class PDFEditor : Form
    {
        public PDFEditor()
        {
            InitializeComponent();
        }
        private void button_Edit_Click(object sender, EventArgs e)
        {
            SplitPDF();
        }
        private void button_ptw_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取应用程序根目录路径
                string rootPath = AppDomain.CurrentDomain.BaseDirectory;

                // 输入PDF文件路径(放在应用程序根目录)
                string inputPdf = Path.Combine(rootPath, "output\\Microsoft Graph REST API v1.0 For SharePoint.pdf");

                // 输出DOCX文件路径(将保存在应用程序根目录)
                string outputDocx = Path.Combine(rootPath, "word\\Microsoft Graph REST API v1.0 For SharePoint.docx");

                Console.WriteLine($"准备转换: {inputPdf}");
                Console.WriteLine($"输出到: {outputDocx}");

                // 检查输入文件是否存在
                if (!File.Exists(inputPdf))
                {
                    Console.WriteLine($"错误: 输入文件 {inputPdf} 不存在");
                    Console.WriteLine("请确保input.pdf文件放在程序同一目录下");
                    return;
                }

                // 转换PDF到DOCX(不显示Word界面)
                PdfToDocxConverter.ConvertUsingInterop(inputPdf, outputDocx, false);

                Console.WriteLine("转换完成！");
                Console.WriteLine($"已生成文件: {outputDocx}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Console.WriteLine("详细信息:");
                Console.WriteLine(ex.ToString());
            }
        }
        static void SplitPDF()
        {
            string inputPdfPath = "input.pdf"; // 输入的PDF文件路径
            string xmlConfigPath = "pagenumber.xml"; // XML配置文件路径
            string outputDirectory = "output"; // 输出PDF文件目录

            try
            {
                // 检查输入文件是否存在
                if (!File.Exists(inputPdfPath))
                {
                    Console.WriteLine("输入的PDF文件不存在！");
                    return;
                }

                if (!File.Exists(xmlConfigPath))
                {
                    Console.WriteLine("XML配置文件不存在！");
                    return;
                }

                // 创建输出目录
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // 读取XML配置文件
                var xmlDoc = XDocument.Load(xmlConfigPath);

                // 解析XML并生成PDF
                foreach (var item in xmlDoc.Descendants("dataA"))
                {
                    string outputPdfName = item.Element("pdfName").Value;
                    int startPage = int.Parse(item.Element("startPage").Value);
                    int endPage = int.Parse(item.Element("endPage").Value);

                    // 生成PDF
                    ExtractPdfPages(inputPdfPath, outputDirectory, outputPdfName, startPage, endPage);

                    Console.WriteLine($"已生成: {outputPdfName}");
                }

                Console.WriteLine("PDF生成完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        }

        static void ExtractPdfPages(string inputPdfPath, string outputDirectory, string outputPdfName, int startPage, int endPage)
        {
            PdfReader reader = null;
            Document document = null;
            PdfCopy copy = null;

            try
            {
                // 打开PDF文件
                reader = new PdfReader(inputPdfPath);

                // 检查页码范围是否有效
                if (startPage < 1 || endPage > reader.NumberOfPages || startPage > endPage)
                {
                    throw new ArgumentException("页码范围无效！");
                }

                // 创建输出文件路径
                string outputPdfPath = Path.Combine(outputDirectory, outputPdfName);

                // 创建一个新的Document对象
                document = new Document();

                // 创建PdfCopy对象，用于写入新的PDF文件
                copy = new PdfCopy(document, new FileStream(outputPdfPath, FileMode.Create));

                // 打开Document
                document.Open();

                // 遍历指定页码范围，将页面添加到新的PDF文件中
                for (int i = startPage; i <= endPage; i++)
                {
                    PdfImportedPage page = copy.GetImportedPage(reader, i);
                    copy.AddPage(page);
                }
            }
            finally
            {
                // 关闭Document
                if (document != null)
                {
                    document.Close();
                }

                // 关闭PdfReader
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
