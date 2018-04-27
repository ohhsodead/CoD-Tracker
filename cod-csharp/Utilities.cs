using System;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Reflection;

namespace COD
{
	internal static class Utilities
	{
		internal const string BO3_URL					= "https://my.callofduty.com/api/papi-client/crm/cod/v2/title/bo3/";
		internal const string IW_URL					= "https://my.callofduty.com/api/papi-client/crm/cod/v2/title/iw/";
		internal const string WWII_URL					= "https://my.callofduty.com/api/papi-client/crm/cod/v2/title/wwii/";

		internal const string BO3_LEADERBOARDS_URL		= "https://my.callofduty.com/api/papi-client/leaderboards/v2/title/bo3/";
		internal const string IW_LEADERBOARDS_URL		= "https://my.callofduty.com/api/papi-client/leaderboards/v2/title/iw/";
		internal const string WWII_LEADERBOARDS_URL		= "https://my.callofduty.com/api/papi-client/leaderboards/v2/title/wwii/";


		internal static bool ValidResponse(string data)
		{
			string validator = @"{
			  'type': 'object',
			  'required': true,
			  'properties': {
				'status': {
					'type': 'string',
					'required': true
				},
				'data': {
				  'type': 'object',
				  'required': true,
				  'properties': {
					'type': {
						'type': 'string',
						'required': true
					},
					'message': {
						'type': 'string',
						'required': true
					}
				  }
				}
			  }
			}";

			//Hack: Just pull in the new library at some point!
#pragma warning disable 0618

			var schema = JsonSchema.Parse(validator);
			var obj = JObject.Parse(data);

			var ret = obj.IsValid(schema);

			return !obj.IsValid(schema);
#pragma warning restore 0618

		}

		internal static string GetDescription(this Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if (name != null)
			{
				FieldInfo field = type.GetField(name);
				if (field != null)
				{
                    if (Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
			}
			return null;
		}

	}
}
