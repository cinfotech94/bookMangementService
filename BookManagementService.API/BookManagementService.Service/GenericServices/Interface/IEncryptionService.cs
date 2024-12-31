using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagementService.Domain.DTO.Common;

namespace BookManagementService.Service.GenericServices.Interface;
public interface IEncryptionService
{
    (string, Exception) Maskpan(string pan);
    string Decrypt(string dataToDecrypt, string key);
    string Encrypt(string dataToEncrypt, string key);
}
