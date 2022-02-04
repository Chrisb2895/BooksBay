using LibraryManager.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace LibraryManager.CustomProviders
{
     class ProtectedConfigurationSection : IConfigurationSection
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IConfigurationSection _section;
        private readonly Lazy<IDataProtector> _protector;

        public ProtectedConfigurationSection(
            IDataProtectionProvider dataProtectionProvider,
            IConfigurationSection section)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _section = section;

            _protector = new Lazy<IDataProtector>(() => dataProtectionProvider.CreateProtector(section.Path));
        }

        public IConfigurationSection GetSection(string key)
        {
            return new ProtectedConfigurationSection(_dataProtectionProvider, _section.GetSection(key));
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _section.GetChildren()
                .Select(x => new ProtectedConfigurationSection(_dataProtectionProvider, x));
        }

        public IChangeToken GetReloadToken()
        {
            return _section.GetReloadToken();
        }

        public string this[string key]
        {
            get => GetProtectedValue(_section[key]);
            set => _section[key] = _protector.Value.Protect(value);
        }

        public string Key => _section.Key;
        public string Path => _section.Path;

        public string Value
        {
            get => GetProtectedValue(_section.Value);
            set => _section.Value = _protector.Value.Protect(value);
        }

        private string GetProtectedValue(string value)
        {
            if (value == null)
                return null;

            return _protector.Value.Unprotect(value);
        }

     
    }
}
}
