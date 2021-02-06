using System;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.ViewModels.Administration.System;

namespace CelloStore.Providers
{
    public class ConfigurationProvider : BaseProvider
    {
        public ConfigurationProvider(EntitiesModel context, IPrincipal principal) : base(context, principal)
        {
        }

        public string GetValue(string key)
        {
            var config = context.Configurations.SingleOrDefault(configuration => configuration.Key == key);
            string result = config == null ? String.Empty : config.Value;            
            return result;
        }

        public T GetValue<T>(string key)
        {
            string result = GetValue(key);
            return (T)Convert.ChangeType(result, typeof(T));
        }


        public void SetValue(string key, string value)
        {
            var configuration = context.Configurations.SingleOrDefault(config => config.Key == key);
            configuration = configuration ?? new Configuration();
            configuration.Key = key;
            configuration.Value = value;
            configuration.ChangedWhen = DateTime.Now;
            configuration.ChangedWho = principal.Identity.Name;
            context.SaveChanges();            
        }

        public void SetValue<T>(string key, T value)
        {
            SetValue(key, Convert.ToString(value));
        }

        public string this[string key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                if (GetValue(key) != value)
                {
                    SetValue(key, value);
                }
            }
        }

        public void UpdateConfigurations(ConfigurationViewModel model)
        {            
            SetValue(ConfigurationKeys.SmtpServer, model.SmtpServer);
            SetValue(ConfigurationKeys.SmtpPort, model.SmtpPort);
            SetValue(ConfigurationKeys.SmtpAuthentication, model.SmtpAuthentication);
            SetValue(ConfigurationKeys.SmtpUserName, model.SmtpUserName);
            SetValue(ConfigurationKeys.SmtpPassword, model.SmtpPassword);
            SetValue(ConfigurationKeys.SmtpSsl, model.SmtpSsl);
            SetValue(ConfigurationKeys.SystemSenderName, model.SystemSenderName);
            SetValue(ConfigurationKeys.SystemSenderAddress, model.SystemSenderAddress);
        }
    }
}
