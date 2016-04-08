using Microsoft.Xna.Framework;
using System;

namespace CSharpQuadTree
{
    public interface IQuadObject
    {
        Rectangle rectangle { get; }
        event EventHandler BoundsChanged;
    }
}