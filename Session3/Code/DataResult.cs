using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session3.Code
{
    public class DataResult
    {
        private bool disposed;

        public int SuccessfulChange { get; set; }
        public int DuplicateRecord { get; set; }
        public int RecordMissing { get; set; }

        public DataResult()
        {
            disposed = false;
            SuccessfulChange = 0;
            DuplicateRecord = 0;
            RecordMissing = 0;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose all used resources.
        /// </summary>
        /// <param name="disposing">Indicates the source call to dispose.</param>
        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                SuccessfulChange = 0;
                DuplicateRecord = 0;
                RecordMissing = 0;
                this.disposed = true;
                ////Number of instance you want to dispose
            }
        }
    }
}
