using System;
using System.Threading.Tasks;

namespace cloudscribe_identity_demo.EmbedWeb
{
    public interface IPowerBIEmbedder
    {
        Task<EmbedInfo> GetEmbedInfo(Guid workspaceId, Guid reportId);
    }
}