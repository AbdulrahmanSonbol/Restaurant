using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Exceptions
{
    public abstract class NotFoundException(string Message) : Exception(Message)
    {
    }
    public sealed class RestaurantNotFoundException(int Id) : NotFoundException($"Restaurant with id {Id} not found.");

}
