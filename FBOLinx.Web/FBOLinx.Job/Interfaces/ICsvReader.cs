using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FBOLinx.Job.Interfaces
{
    public abstract class ICsvReader<TEntity>
    {
        private readonly string _filePath;
        private FileStream _fileStream;
        private StreamReader _streamReader;
        public ICsvReader(string filePath)
        {
            _filePath = filePath;
            _fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000,
                FileOptions.SequentialScan);
            _streamReader = new StreamReader(_fileStream, Encoding.ASCII);
        }

        public void JumpToEnd()
        {
            if (_streamReader == null)
                return;
            _streamReader.BaseStream.Seek(0, SeekOrigin.End);
        }

        public List<TEntity> GetRecords(int skipLines = 0)
        {
            try
            {
                System.Exception lastException = null;

                // Skip
                for (int i = 0; i < skipLines; i++)
                {
                    _streamReader.ReadLine();
                }

                List<TEntity> records = new List<TEntity>();

                // Read All Lines
                while (!_streamReader.EndOfStream)
                {
                    try
                    {
                        var line = _streamReader.ReadLine();
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

        public void Dispose()
        {
            _streamReader.Dispose();
            _fileStream.Dispose();
        }

        public abstract TEntity ParseCsvLineToEntity(string line);
    }
}
