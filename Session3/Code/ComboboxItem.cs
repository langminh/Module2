using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session3.Code
{
    public class ComboboxItem
    {
        private bool disposed;
        public string Text { get; set; }
        public int Value { get; set; }

        public ComboboxItem()
        {
            disposed = false;
        }

        public override string ToString()
        {
            return Text;
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
                Text = string.Empty;
                Value = 0;
                this.disposed = true;
                ////Number of instance you want to dispose
            }
        }
    }
}
