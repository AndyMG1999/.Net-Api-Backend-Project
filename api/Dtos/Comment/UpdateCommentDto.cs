using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentDto
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
    }
}