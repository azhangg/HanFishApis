using Entities.Message;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Module.Message
{
    public interface IChatMessageRepository : IRepository<ChatMessage>
    {
    }
}
