using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.DataProtectors
{
    public class AesDataProtectionProvider : IDataProtectionProvider
    {
        AesDataProtector _dataProtector;
        public AesDataProtectionProvider(AesDataProtector dataProtector)
        {
            _dataProtector = dataProtector;
        }
        public IDataProtector Create(params string[] purposes)
        {
            return _dataProtector;
        }
    }
}
