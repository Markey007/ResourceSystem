using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceSystem
{
    public interface IDataCollectionTemplete
    {

        Type GetContenType();

        void ConvertToContentList(List<IDataTemplete> contents);
    }
}
