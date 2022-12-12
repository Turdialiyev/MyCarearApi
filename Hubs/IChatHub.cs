using MyCarearApi.Entities;
using System.Collections;

namespace MyCarearApi.Hubs
{
    public interface IChatHub
    {
        void RecieveMessage(Message message);
        void InitializeCHats(IList profils);
    }
}
