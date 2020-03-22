using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
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
    public class LZWController : ControllerBase
    {
        readonly IHistoryManager History;
        public LZWController(IHistoryManager History)
        {
            this.History = History;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        //----------------------------------------------------------LZW-------------------------------------------------


        [HttpPost("Compress", Name = "PostCompressLZW")]
        public void Post(string RServerPath, string newname)
        {
            string WServerPath = "";
            if (RServerPath != "")
            {
                /*All the pre-compress path preparation*/
                LZWCompressor lz = new LZWCompressor();
                string ServerDirectory = Directory.GetCurrentDirectory();
                string path = Path.Combine(ServerDirectory, "Compress/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                WServerPath = path + newname + ".LZW";
                /*Compress the file*/
                lz.Compress(RServerPath, WServerPath);

                /*adds the compression to the history*/
                string HistLine = lz.GetFilesMetrics(Path.GetFileName(RServerPath), RServerPath, WServerPath);
                History.AddToHistory(HistLine);
                this.WriteOnHistory(HistLine);
            }
        }
        [HttpPost("Decompress", Name = "PostDecompressLWZ")]
<<<<<<< HEAD
        public void Post(string RServerPath)
        {
            string WServerPath = "";
            if (RServerPath != "")
            {
                /*All the pre-compress path preparation*/
                LZWCompressor lz = new LZWCompressor();
                string ServerDirectory = Directory.GetCurrentDirectory();
                string path = Path.Combine(ServerDirectory, "Decompress/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                WServerPath = path + Path.GetFileName(RServerPath);
                /*Compress the file*/
                lz.Decompress(RServerPath, WServerPath);

                /*adds the compression to the history*/
                string HistLine = lz.GetFilesMetrics(Path.GetFileName(RServerPath), RServerPath, WServerPath);
                History.AddToHistory(HistLine);
                this.WriteOnHistory(HistLine);
            }
        }
=======
        
>>>>>>> 79ef9c4ecf2ac676f02f6f1104c089a5bca925c7

        //----------------------------------------------------------Extras-------------------------------------------------
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