using System;
using System.Collections.Generic;
using System.Reflection;
using LOGIC.Services.Models.CoreAdminDataModels;
using Microsoft.EntityFrameworkCore;

namespace BooksBay.Areas.Admin.ViewModels
{
    public class DataListViewModel
    {
        public Type EntityType { get; internal set; }
        public IEnumerable<object> Data { get; internal set; }
        public DbContext DbContext { get; internal set; }
        public PropertyInfo DbSetProperty { get; internal set; }

        public CoreAdminDataIndex DbModelInfo { get;  set; }
    }
}
