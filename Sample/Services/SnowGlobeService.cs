using System.Collections.Generic;
using MongoDB.Driver;
using Ornaments.Data;

namespace Ornaments
{
    public class SnowGlobeService
    {
        private readonly OrnamentsContext _ornamentsContext;

        public SnowGlobeService(OrnamentsContext ornamentsContext)
        {
            _ornamentsContext = ornamentsContext;
        }

        public IEnumerable<SnowGlobe> GetSnowGlobes() => _ornamentsContext.SnowGlobes
                        .Find(snowGlobe => snowGlobe.Name.StartsWith("Winter")).ToEnumerable();
    }
}