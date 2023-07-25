﻿namespace TradingBot.Backend.Services.Identity.Api.Dtos
{
    public class SelectList<TKey>
    {
        public SelectList(TKey id, string description)
        {
            Id = id;
            Description = description;
        }

        public TKey Id { get; }

        public string Description { get; }
    }
}
