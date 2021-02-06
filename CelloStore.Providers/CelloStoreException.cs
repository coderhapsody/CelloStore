using System;

namespace CelloStore.Providers
{
    [Serializable]
    public class CelloStoreException : Exception
    {
        public CelloStoreException(string message) : base(message)
        {            
        }
    }
}
