using System;

namespace DapRepo.DataAccess
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotDbGeneratedAttribute : Attribute
    {
    }
}