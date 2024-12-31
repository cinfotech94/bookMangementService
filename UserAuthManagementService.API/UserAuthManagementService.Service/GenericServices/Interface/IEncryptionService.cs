using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthManagementService.Domain.DTO.Common;

namespace UserAuthManagementService.Service.GenericServices.Interface;
public interface IEncryptionService
{
    (string, Exception) Maskpan(string pan,string caller, string correltionId);
    string Decrypt(string dataToDecrypt, string key);
    string Encrypt(string dataToEncrypt, string key);
}
