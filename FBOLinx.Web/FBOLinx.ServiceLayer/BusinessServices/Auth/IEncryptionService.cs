using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.BusinessServices.Auth
{
    public interface IEncryptionService
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);

    }
}
