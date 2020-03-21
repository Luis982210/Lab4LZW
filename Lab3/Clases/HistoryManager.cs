using Lab3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Clases
{
    public interface IHistoryManager
    {
        void AddToHistory(string x);
        List<FileModel> GetHistory();
    }

    public class HistoryManager : IHistoryManager
    {
        Stack<FileModel> History = new Stack<FileModel>();

        public void AddToHistory(string Text)
        {
            string[] Values = Text.Split('|');
            FileModel FL = new FileModel()
            {
                Name = Values[0],
                RazonDeComprecion = double.Parse(Values[1]),
                FactorDeComprecion = double.Parse(Values[2]),
                PorcentajeDeReduccion = double.Parse(Values[3])
            };
            History.Push(FL);
        }

        public List<FileModel> GetHistory()
        {
            return History.ToList();
        }
    }
}
