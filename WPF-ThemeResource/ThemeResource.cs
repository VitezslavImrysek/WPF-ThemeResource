using System;
using System.Windows;
using System.Windows.Markup;

namespace WPF_ThemeResource
{
    public class ThemeResource : DynamicResourceExtension
    {
        public ThemeResource() 
        {

        }

        public ThemeResource(ThemeResourceKey resourceKey)
            : base(resourceKey.ToString())
        {

        }

        [ConstructorArgument("resourceKey")]
        public new ThemeResourceKey ResourceKey
        {
            get
            {
                ThemeResourceKey result;
                Enum.TryParse<ThemeResourceKey>((string)base.ResourceKey, out result);

                return result;
            }
            set => base.ResourceKey = value.ToString();
        }
    }
}
