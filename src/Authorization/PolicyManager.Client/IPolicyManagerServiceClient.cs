﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyManager.DataAccess.Models;
using Refit;

namespace PolicyManager.Client
{
    public interface IPolicyManagerServiceClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/api/things")]
        Task<IEnumerable<Thing>> GetThingsAsync();
    }
}