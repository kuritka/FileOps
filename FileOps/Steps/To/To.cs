﻿using FileOps.Configuration.Entities;
using FileOps.Pipe;
using System;
using System.Collections.Generic;

namespace FileOps.Steps.To
{
    public class To : IStep<IAggregate, IAggregate>
    {
        private readonly ToSettings _settings;

        public To(ToSettings settings)
        {
            _settings = settings;
        }

        public IAggregate Execute(IAggregate toProcess)
        {
            throw new NotImplementedException();
        }
    }
}
