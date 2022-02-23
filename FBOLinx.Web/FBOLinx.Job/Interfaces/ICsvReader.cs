using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FBOLinx.Job.Interfaces
{
    public abstract class ICsvReader<TEntity>
    {
        private readonly string _filePath;
        public ICsvReader(string filePath)
        {
            _filePath = filePath;
        }

        public List<TEntity> GetRecords(int skip = 0)
        {
            try
            {
                System.Exception lastException = null;
                using FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan);
                using StreamReader sr = new StreamReader(fs, Encoding.ASCII);

                // Skip
                for (int i = 0; i < skip; i++)
                {
                    sr.ReadLine();
                }

                List<TEntity> records = new List<TEntity>();

                // Read All Lines
                while (!sr.EndOfStream)
                {
                    try
                    {
                        var line = sr.ReadLine();
                        records.Add(ParseCsvLineToEntity(line));
                    }
                    catch (System.Exception exception)
                    {
                        lastException = exception;
                        //Continue like normal...
                    }
                }

                if (records.Count == 0 && lastException != null)
                    throw lastException;

                return records;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw err;
            }
        }

        public string GetSafeField(string[] fields, int index)
        {
            if (fields.Length <= index)
            {
                return "";
            }
            return fields[index].Trim();
        }

        public abstract TEntity ParseCsvLineToEntity(string line);
    }
}
