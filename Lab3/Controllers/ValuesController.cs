using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using Lab3.Clases;

namespace Lab3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HuffmanController : ControllerBase
    {
        readonly IHistoryManager History;
        //constructer
        public HuffmanController(IHistoryManager History)
        {
            this.History = History;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //localhost:51626/api/Values/GetWithParam/?nombre=""
        //[HttpGet("GetWithParam", Name = "Get")]        
        
        // POST api/values
        [HttpPost("Compress",Name= "PostCompressHuffman")]
        public void Post(string RServerPath, string newname)
        {
            //var UploadedFile = Request.Form.Files[0];
            string WServerPath = "";
            if (RServerPath != "")
            {
                HuffmanCoder hf = new HuffmanCoder();
                string ServerDirectory = Directory.GetCurrentDirectory();
                string path = Path.Combine(ServerDirectory, "Compress/");
                if (!Directory.Exists(path)) 
                    Directory.CreateDirectory(path);
                WServerPath = path + newname + ".huff";
                //FileStream ServerFile = new FileStream(RServerPath, FileMode.Create, FileAccess.ReadWrite);
                //UploadedFile.CopyTo(ServerFile);
                //ServerFile.Close();
                hf.Compress(RServerPath, WServerPath);
                string HistLine = hf.GetFilesMetrics(Path.GetFileName(RServerPath), RServerPath, WServerPath);
                History.AddToHistory(HistLine);
                this.WriteOnHistory(HistLine);
            }
        }
        // POST api/values
        [HttpPost("Decompress", Name = "PostDecompressHufman")]
        public void Post(string RServerPath)
        {
            //var UploadedFile = Request.Form.Files[0];
            string WServerPath = "";
            if (RServerPath != "")
            {
                HuffmanCoder hf = new HuffmanCoder();
                string ServerDirectory = Directory.GetCurrentDirectory();
                string path = Path.Combine(ServerDirectory, "Decompress/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                
                WServerPath = path + Path.GetFileName(RServerPath);
                //FileStream ServerFile = new FileStream(RServerPath, FileMode.Create, FileAccess.ReadWrite);
                //UploadedFile.CopyTo(ServerFile);
                //ServerFile.Close();
                string file = hf.uncompress(RServerPath);
                using (FileStream FileToWrite = new FileStream(WServerPath, FileMode.CreateNew))
                {
                    FileToWrite.Write(Encoding.Default.GetBytes(file));
                }
                string HistLine = hf.GetFilesMetrics(Path.GetFileName(RServerPath), RServerPath, WServerPath);
                History.AddToHistory(HistLine);
                this.WriteOnHistory(HistLine);
            }
        }
        private void WriteOnHistory(string NewLine)
        {
            string HistoryPath = Path.Combine(Directory.GetCurrentDirectory() + "/History.txt");
            using (StreamReader Sr = new StreamReader(HistoryPath))
            {
                NewLine = (Sr.ReadToEnd() + "\r\n" + NewLine).Trim();
            }

            using (StreamWriter Sw = new StreamWriter(new FileStream(HistoryPath, FileMode.Create)))
            {
                Sw.Write(NewLine);
            }
        }
    }
}
