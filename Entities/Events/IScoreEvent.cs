using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public interface IScoreEvent
    {
        int Points { get; }
    }
}
