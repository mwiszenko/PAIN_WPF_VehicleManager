using System;

namespace Vehicles.MVVM
{
    public interface IViewModel
    {
        Action Close { get; set; }
    }
}
