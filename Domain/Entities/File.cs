using Domain.Entities.Base;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    [BsonCollection("File")]
    public class Files : Document
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Content { get; set; }
        public bool InProcess { get; set; }

        public Files AddFile(IFormFile file)
        {
            Name = file.Name;
            Extension = file.ContentType;
            InProcess = true;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                Content = Convert.ToBase64String(fileBytes);
            }
            return this;
        }
        public void UpdateProcess()
        {
            InProcess = false;
        }
    }
}