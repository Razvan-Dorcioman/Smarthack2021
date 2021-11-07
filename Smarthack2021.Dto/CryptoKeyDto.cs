using Smarthack2021.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smarthack2021.Dto
{
    public class CryptoKeyDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PrivateKey { get; set; }

        public string PublicKey { get; set; }

        public CryptoType Type { get; set; }
    }
}
